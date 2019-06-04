using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XPlaneSituationTrainer.Lib.Commanding;
using XPlaneSituationTrainer.Lib.Connectivity;

namespace XPlaneSituationTrainer.Lib.Commanding {
    public class XPCDirector {
        public XPCDirector() {
        }

        /// <summary>
        /// Pauses or unpauses X-Plane.
        /// </summary>
        /// <param name="pause">{@code true} to pause the simulator; {@code false} to un-pause.</param>
        /// <exception cref="IOException">If the command cannot be sent.</exception>
        public void PauseSim(bool pause) {
            //            S     I     M     U     LEN   VAL
            byte[] msg = { 0x53, 0x49, 0x4D, 0x55, 0x00, 0x00 };
            msg[5] = (byte)(pause ? 0x01 : 0x00);
            XPCConnector.Instance.Send(msg);
        }

        /// <summary>
        /// Pauses, unpauses, or switches the pause state of X-Plane.
        /// </summary>
        /// <param name="pause">{@code 1} to pause the simulator, {@code 0} to unpause, or {@code 2} to switch.</param>
        /// <exception cref="ArgumentException">If the values of {@code pause} is not a valid command.</exception>
        /// <exception cref="IOException">If the command cannot be sent.</exception>
        public void PauseSim(int pause) {
            if (pause < 0 || pause > 2) {
                throw new ArgumentException("pause must be a value in the range [0, 2].");
            }

            //            S     I     M     U     LEN   VAL
            byte[] msg = { 0x53, 0x49, 0x4D, 0x55, 0x00, 0x00 };
            msg[5] = (byte)pause;
            XPCConnector.Instance.Send(msg);
        }

        /// <summary>
        /// Requests a single dref value from X-Plane.
        /// </summary>
        /// <returns>A byte array representing data dependent on the dref requested.</returns>
        /// <param name="dref">The name of the dref requested.</param>
        /// <exception cref="IOException">If either the request or the response fails.</exception>
        public float[] GetDREF(string dref) {
            return GetDREFs(new string[] { dref })[0];
        }

        /// <summary>
        /// Requests several dref values from X-Plane.
        /// </summary>
        /// <returns>A multidimensional array representing the data for each requested dref.</returns>
        /// <param name="drefs">An array of dref names to request.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IOException">If either the request or the response fails.</exception>
        public float[][] GetDREFs(string[] drefs) {
            //Preconditions
            if (drefs == null || drefs.Length == 0) {
                throw new ArgumentException("drefs must be a valid array with at least one dref.");
            }

            if (drefs.Length > 255) {
                throw new ArgumentException("Can not request more than 255 DREFs at once.");
            }

            //Convert drefs to bytes.
            byte[][] drefBytes = new byte[drefs.Length][];

            for (int i = 0; i < drefs.Length; ++i) {
                drefBytes[i] = Encoding.UTF8.GetBytes(drefs[i]);

                if (drefBytes[i].Length == 0) {
                    throw new ArgumentException("DREF " + i + " is an empty string!");
                }

                if (drefBytes[i].Length > 255) {
                    throw new ArgumentException("DREF " + i + " is too long (must be less than 255 bytes in UTF-8). Are you sure this is a valid DREF?");
                }
            }

            //Build and send message
            List<byte> sendMsg = new List<byte>();
            sendMsg.AddRange(Encoding.UTF8.GetBytes("GETD"));
            sendMsg.Add(0xFF);
            sendMsg.Add((byte)drefs.Length);

            foreach (byte[] dref in drefBytes) {
                sendMsg.Add((byte)dref.Length);
                sendMsg.AddRange(dref);
            }

            XPCConnector.Instance.Send(sendMsg.ToArray());

            //Read response
            byte[] data = readUDP();
            if (data.Length == 0) {
                throw new IOException("No response received.");
            }

            if (data.Length < 6) {
                throw new IOException("Response too short");
            }

            float[][] result = new float[drefs.Length][];
            ByteBuffer bb = ByteBuffer.wrap(data);
            bb.order(ByteOrder.LITTLE_ENDIAN);
            int cur = 6;

            for (int j = 0; j < result.Length; ++j) {
                result[j] = new float[data[cur++]];
                for (int k = 0; k < result[j].Length; ++k) //TODO: There must be a better way to do this
                {
                    result[j][k] = bb.getFloat(cur);
                    cur += 4;
                }
            }
            return result;
        }

        public void SendDREF(string dref, float value) {
            SendDREF(dref, new float[] { value });
        }

        /// <summary>
        /// Sends a command to X-Plane that sets the given DREF.
        /// </summary>
        /// <param name="dref">The name of the X-Plane dataref to set.</param>
        /// <param name="value">An array of floating point values whose structure depends on the dref specified.</param>
        /// <exception cref="IOException">If the command cannot be sent.</exception>
        public void SendDREF(string dref, float[] value) {
            SendDREFs(new string[] { dref }, new float[][] { value });
        }

        /// <summary>
        /// Sends a command to X-Plane that sets the given DREF.
        /// </summary>
        /// <param name="drefs">The names of the X-Plane datarefs to set.</param>
        /// <param name="values">A sequence of arrays of floating point values whose structure depends on the drefs specified.</param>
        /// <exception cref="IOException">If the command cannot be sent.</exception>
        public void SendDREFs(string[] drefs, float[][] values) {
            //Preconditions
            if (drefs == null || drefs.Length == 0) {
                throw new ArgumentException(("drefs must be non-empty."));
            }

            if (values == null || values.Length != drefs.Length) {
                throw new ArgumentException("values must be of the same size as drefs.");
            }

            ByteArrayOutputStream os = new ByteArrayOutputStream();
            os.write("DREF".getBytes(StandardCharsets.UTF_8));
            os.write(0xFF); //Placeholder for message Length

            for (int i = 0; i < drefs.Length; ++i) {
                string dref = drefs[i];
                float[] value = values[i];

                if (dref == null) {
                    throw new ArgumentException("dref must be a valid string.");
                }
                if (value == null || value.Length == 0) {
                    throw new ArgumentException("value must be non-null and should contain at least one value.");
                }

                //Convert drefs to bytes.
                byte[] drefBytes = dref.getBytes(StandardCharsets.UTF_8);
                if (drefBytes.Length == 0) {
                    throw new ArgumentException("DREF is an empty string!");
                }
                if (drefBytes.Length > 255) {
                    throw new ArgumentException("dref must be less than 255 bytes in UTF-8. Are you sure this is a valid dref?");
                }

                ByteBuffer bb = ByteBuffer.allocate(4 * value.Length);
                bb.order(ByteOrder.LITTLE_ENDIAN);

                for (int j = 0; j < value.Length; ++j) {
                    bb.putFloat(j * 4, value[j]);
                }

                //Build and send message
                os.write(drefBytes.Length);
                os.write(drefBytes, 0, drefBytes.Length);
                os.write(value.Length);
                os.write(bb.array());
            }

            XPCConnector.Instance.Send(os.toByteArray());
        }

        /// <summary>
        /// Gets the control surface information for the specified airplane.
        /// </summary>
        /// <returns>An array containing control surface data in the same format as {@code sendCTRL}.</returns>
        /// <param name="ac">The aircraft to get control surface information for.</param>
        /// <exception cref="IOException">If the command cannot be sent or a response cannot be read.</exception>
        public float[] GetCTRL(int ac) {
            // Send request
            List<byte> sendMsg = new List<byte>();
            sendMsg.AddRange(Encoding.UTF8.GetBytes("GETC"));
            sendMsg.Add(0xFF); //Placeholder for message Length
            os.write(); 
            os.write(ac);
            XPCConnector.Instance.Send(os.toByteArray());

            // Read response
            byte[] data = XPCConnector.Instance.Receive();
            if (data.Length == 0) {
                throw new IOException("No response received.");
            }
            if (data.Length < 31) {
                throw new IOException("Response too short");
            }

            // Parse response
            float[] result = new float[7];
            ByteBuffer bb = ByteBuffer.wrap(data);
            bb.order(ByteOrder.LITTLE_ENDIAN);
            result[0] = bb.getFloat(5);
            result[1] = bb.getFloat(9);
            result[2] = bb.getFloat(13);
            result[3] = bb.getFloat(17);
            result[4] = bb.get(21);
            result[5] = bb.getFloat(22);
            result[6] = bb.getFloat(27);
            return result;
        }

        /// <summary>
        /// Sends command to X-Plane setting control surfaces on the player ac.
        /// </summary>
        /// <param name="values"><p>An array containing zero to six values representing control surface data as follows:</p>
        ///              <ol>
        ///                  <li>Latitudinal Stick[-1, 1]</li>
        ///                  <li>Longitudinal Stick[-1, 1]</li>
        ///                  <li>Rudder Pedals[-1, 1]</li>
        ///                  <li>Throttle[-1, 1]</li>
        ///                  <li>Gear(0=up, 1=down)</li>
        ///                  <li>Flaps[0, 1]</li>
        ///                  <li>Speedbrakes[-0.5, 1.5]</li>
        ///              </ol>
        ///              <p>
        ///                  If @{code ctrl} is less than 6 elements long. The missing elements will not be changed. To change values in the middle of the array without affecting the preceding values, set the preceding values to -998.
        ///              </p></param>
        public void SendCTRL(float[] values) {
            SendCTRL(values, 0);
        }

        /// <summary>
        /// Sends command to X-Plane setting control surfaces on the specified ac.
        /// </summary>
        /// <param name="values"><p>An array containing zero to six values representing control surface data as follows:</p>
        ///                <ol>
        ///                    <li>Latitudinal Stick[-1, 1]</li>
        ///                    <li>Longitudinal Stick[-1, 1]</li>
        ///                    <li>Rudder Pedals[-1, 1]</li>
        ///                    <li>Throttle[-1, 1]</li>
        ///                    <li>Gear(0=up, 1=down)</li>
        ///                    <li>Flaps[0, 1]</li>
        ///                    <li>Speedbrakes[-0.5, 1.5]</li>
        ///                </ol>
        ///                <p>
        ///                    If @{code ctrl} is less than 6 elements long, The missing elements will not be changed. To change values in the middle of the array without affecting the preceding values, set the preceding values to -998.
        ///                </p></param>
        /// <param name="ac">Ac.</param>
        /// <exception cref="IOException">If the command cannot be sent or a response cannot be read.</exception>
        public void SendCTRL(float[] values, int ac) {
            //Preconditions
            if (values == null) {
                throw new ArgumentException("ctrl must no be null.");
            }
            if (values.Length > 7) {
                throw new ArgumentException("ctrl must have 7 or fewer elements.");
            }
            if (ac < 0 || ac > 9) {
                throw new ArgumentException("ac must be non-negative and less than 9.");
            }

            //Pad command values and convert to bytes
            int i;
            int cur = 0;
            ByteBuffer bb = ByteBuffer.allocate(26);
            bb.order(ByteOrder.LITTLE_ENDIAN);
            for (i = 0; i < 6; ++i) {
                if (i == 4) {
                    if (i >= values.Length) {
                        bb.put(cur, (byte)-1);
                    } else {
                        bb.put(cur, (byte)values[i]);
                    }
                    cur += 1;
                } else if (i >= values.Length) {
                    bb.putFloat(cur, -998);
                    cur += 4;
                } else {
                    bb.putFloat(cur, values[i]);
                    cur += 4;
                }
            }
            bb.put(cur++, (byte)ac);
            bb.putFloat(cur, values.Length == 7 ? values[6] : -998);

            //Build and send message
            ByteArrayOutputStream os = new ByteArrayOutputStream();
            os.write("CTRL".getBytes(StandardCharsets.UTF_8));
            os.write(0xFF); //Placeholder for message Length
            os.write(bb.array());
            sendUDP(os.toByteArray());
        }

        /// <summary>
        /// Gets position information for the specified airplane.
        /// </summary>
        /// 
        /// <param name="ac">The aircraft to get position information for.</param>
        /// <returns>An array containing control surface data in the same format as {@code sendPOSI}.</returns>
        /// <exception cref="IOException">If the command cannot be sent or a response cannot be read.</exception>
        public double[] GetPOSI(int ac) {
            // Send request
            XPCConnector.Instance.Send(PackValues("GETP", 0xFF, ac).ToArray());

            // Read response
            byte[] data = readUDP();
            if (data.Length == 0) {
                throw new IOException("No response received.");
            }
            if (data.Length < 34) {
                throw new IOException("Response too short");
            }

            // Parse response
            double[] result = new double[7];

            if (BitConverter.IsLittleEndian) {
                Array.Reverse(data);
            }

            for (int i = 0; i < 7; ++i) {
                result[i] = BitConverter.ToDouble(data, 6 + 4 * i);
            }
            return result;
        }

        /**
         * Sets the position of the player ac.
         *
         * @param values   <p>An array containing position elements as follows:</p>
         *                 <ol>
         *                     <li>Latitude (deg)</li>
         *                     <li>Longitude (deg)</li>
         *                     <li>Altitude (m above MSL)</li>
         *                     <li>Roll (deg)</li>
         *                     <li>Pitch (deg)</li>
         *                     <li>True Heading (deg)</li>
         *                     <li>Gear (0=up, 1=down)</li>
         *                 </ol>
         *                 <p>
         *                     If @{code ctrl} is less than 6 elements long, The missing elements will not be changed. To
         *                     change values in the middle of the array without affecting the preceding values, set the
         *                     preceding values to -998.
         *                 </p>
         * @throws IOException If the command can not be sent.
         */
        public void SendPOSI(float[] values) {
            SendPOSI(values, 0);
        }

        /**
         * Sets the position of the specified ac.
         *
         * @param values   <p>An array containing position elements as follows:</p>
         *                 <ol>
         *                     <li>Latitude (deg)</li>
         *                     <li>Longitude (deg)</li>
         *                     <li>Altitude (m above MSL)</li>
         *                     <li>Roll (deg)</li>
         *                     <li>Pitch (deg)</li>
         *                     <li>True Heading (deg)</li>
         *                     <li>Gear (0=up, 1=down)</li>
         *                 </ol>
         *                 <p>
         *                     If @{code ctrl} is less than 6 elements long, The missing elements will not be changed. To
         *                     change values in the middle of the array without affecting the preceding values, set the
         *                     preceding values to -998.
         *                 </p>
         * @param ac The ac to set. 0 for the player ac.
         * @throws IOException If the command can not be sent.
         */
        public void SendPOSI(float[] values, int ac) {
            //Preconditions
            if (values == null) {
                throw new ArgumentException("posi must no be null.");
            }
            if (values.Length > 7) {
                throw new ArgumentException("posi must have 7 or fewer elements.");
            }
            if (ac < 0 || ac > 255) {
                throw new ArgumentException("ac must be between 0 and 255.");
            }

            //Pad command values and convert to bytes
            int i;

            byte[] bb = new byte[28];

            for (i = 0; i < values.Length; ++i) {
                bb.putFloat(i * 4, values[i]);
            }
            for (; i < 7; ++i) {
                bb.putFloat(i * 4, -998);
            }

            //Build and send message
            os.write(bb.array());
            XPCConnector.Instance.Send(PackValues("POSI", 0xFF, ac, ).ToArray());
        }

        /**
         * Reads X-Plane data
         *
         * @return The data read.
         * @throws IOException If the read operation fails.
         */
        public float[][] ReadData() {
            byte[] buffer = readUDP();
            ByteBuffer bb = ByteBuffer.wrap(buffer);
            int cur = 5;
            int len = bb.get(cur++);
            float[][] result = new float[bb.get(len)][9];
            for (int i = 0; i < len; ++i) {
                for (int j = 0; j < 9; ++j) {
                    result[i][j] = bb.getFloat(cur);
                    cur += 4;
                }
            }
            return result;
        }

        /**
         * Sends data to X-Plane
         *
         * @param data The data to send.
         * @throws IOException If the command cannot be sent.
         */
        public void SendDATA(float[][] data) {
            //Preconditions
            if (data == null || data.Length == 0) {
                throw new ArgumentException("data must be a non-null, non-empty array.");
            }

            //Convert data to bytes
            ByteBuffer bb = ByteBuffer.allocate(4 * 9 * data.Length);
            bb.order(ByteOrder.LITTLE_ENDIAN);
            for (int i = 0; i < data.Length; ++i) {
                int rowStart = 9 * 4 * i;
                float[] row = data[i];
                if (row.Length != 9) {
                    throw new ArgumentException("Rows must contain exactly 9 items. (Row " + i + ")");
                }

                bb.putInt(rowStart, (int)row[0]);
                for (int j = 1; j < row.Length; ++j) {
                    bb.putFloat(rowStart + 4 * j, row[j]);
                }
            }

            //Build and send message
            os.write(bb.array());
            XPCConnector.Instance.Send(PackValues("DATA", 0xFF, ).ToArray());
        }

        /**
         * Selects what data X-Plane will export over UDP.
         *
         * @param rows The row numbers to select.
         * @throws IOException If the command cannot be sent.
         */
        public void SelectDATA(int[] rows) {
            //Preconditions
            if (rows == null || rows.Length == 0) {
                throw new ArgumentException("rows must be a non-null, non-empty array.");
            }

            //Convert data to bytes
            ByteBuffer bb = ByteBuffer.allocate(4 * rows.Length);
            bb.order(ByteOrder.LITTLE_ENDIAN);
            for (int i = 0; i < rows.Length; ++i) {
                bb.putInt(i * 4, rows[i]);
            }

            //Build and send message
            os.write(bb.array());
            XPCConnector.Instance.Send(PackValues("DSEL", 0xFF, ).ToArray());
        }

        /**
         * Sets a message to be displayed on the screen in X-Plane at the default screen location.
         *
         * @param msg The message to display. Should not contain any newline characters.
         * @throws IOException If the command cannot be sent.
         */
        public void SendTEXT(string msg) {
            SendTEXT(msg, -1, -1);
        }

        /**
         * Sets a message to be displayed on the screen in X-Plane at the specified coordinates.
         *
         * @param msg The message to display. Should not contain any newline characters.
         * @param x   The number of pixels from the right edge of the screen to display the text.
         * @param y   The number of pixels from the bottom edge of the screen to display the text.
         * @throws IOException If the command cannot be sent.
         */
        public void SendTEXT(string msg, int x, int y) {
            //Preconditions
            if (msg == null) {
                msg = "";
            }

            //Convert drefs to bytes.
            byte[] msgBytes = msg.getBytes(StandardCharsets.UTF_8);
            if (msgBytes.Length > 255) {
                throw new ArgumentException("msg must be less than 255 bytes in UTF-8.");
            }

            ByteBuffer bb = ByteBuffer.allocate(8);
            bb.order(ByteOrder.LITTLE_ENDIAN);
            bb.putInt(0, x);
            bb.putInt(4, y);

            //Build and send message
            os.write(bb.array());
            XPCConnector.Instance.Send(PackValues("TEXT", 0xFF, , msgBytes.Length, msgBytes).ToArray());
        }

        /**
         * Sets the camera view in X-Plane.
         *
         * @param view The view to use.
         * @throws IOException If the command cannot be sent.
         */
        public void SendVIEW(ViewType view) {
            Array bytes = BitConverter.GetBytes((int)view);

            if (BitConverter.IsLittleEndian) {
                Array.Reverse(bytes);
            }

            //Build and send message
            XPCConnector.Instance.Send(PackValues("VIEW", 0xFF, bytes).ToArray());
        }

        /**
         * Adds, removes, or clears a set of waypoints. If the command is clear, the points are ignored
         * and all points are removed.
         *
         * @param op     The operation to perform.
         * @param points An array of values representing points. Each triplet in the array will be
         *               interpreted as a (Lat, Lon, Alt) point.
         * @throws IOException  If the command cannot be sent.
         */
        public void SendWYPT(WaypointOp op, float[] points) {
            //Preconditions
            if (points.Length % 3 != 0) {
                throw new ArgumentException("points.Length should be divisible by 3.");
            }
            if (points.Length / 3 > 255) {
                throw new ArgumentException("Too many points. Must be less than 256.");
            }

            //Convert points to bytes
            ByteBuffer bb = ByteBuffer.allocate(4 * points.Length);
            bb.order(ByteOrder.LITTLE_ENDIAN);
            for (float f : points) {
                bb.putFloat(f);
            }

            //Build and send message
            os.write(bb.array());
            XPCConnector.Instance.Send(PackValues("WYPT", 0xFF, (int)op, points.Length / 3, ).ToArray);
        }

        /**
         * Sets the port on which the client will receive data from X-Plane.
         *
         * @param port The new incoming port number.
         * @throws IOException If the command cannot be sent.
         */
        public void SetCONN(int port) {
            if (port < 0 || port >= 0xFFFF) {
                throw new ArgumentException("Invalid port (must be non-negative and less than 65536).");
            }

            XPCConnector.Instance.Send(PackValues("CONN", 0xFF, port, port >> 8).ToArray());

            int soTimeout = socket.getSoTimeout();
            socket.close();
            socket = new DatagramSocket(port);
            socket.setSoTimeout(soTimeout);
            readUDP(); // Try to read response
        }

        private List<byte> PackValues(params object[] values) {
            List<byte> msg = new List<byte>();

            foreach (object v in values) {
                msg.AddRange(Encoding.UTF8.GetBytes(v.ToString()));
            }

            return msg;
        }
    }
}
