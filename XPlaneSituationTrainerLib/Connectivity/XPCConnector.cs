using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace XPlaneSituationTrainer.Lib.Connectivity {
    public class XPCConnector {
        #region Attributes
        private static XPCConnector _instance;
        private IPEndPoint _serverEndPoint;
        #endregion

        #region Properties
        public static XPCConnector Instance {
            get {
                if (_instance == null) {
                    _instance = new XPCConnector();
                }

                return _instance;
            }
        }

        private UdpClient UDPClient { get; set; }
        public bool Connected { get => _serverEndPoint != null; }
        #endregion

        private XPCConnector() {
            UDPClient = new UdpClient();
        }

        public void ConnectTo(string hostname, int port) {
            _serverEndPoint = new IPEndPoint(Convert.ToInt64(hostname), port);
            UDPClient.Connect(_serverEndPoint.Address.ToString(), _serverEndPoint.Port);
        }

        public void Send(string message) {
            Send(Encoding.UTF8.GetBytes(message));
        }

        public void Send(byte[] message) {
            if (Connected) {
                UDPClient.Send(message, message.Length);
            }
        }

        public byte[] Receive() {
            byte[] buffer = new byte[65536];
            
            try {
                buffer = UDPClient.Receive(ref _serverEndPoint);
            } catch(Exception) {
                return null;
            }
            
            return buffer;
        }

        public bool ChangePort(int port) {
            int timeout = UDPClient.Client.ReceiveTimeout;
            _serverEndPoint.Port = port;

            UDPClient.Close();
            UDPClient.Connect(_serverEndPoint.Address.ToString(), _serverEndPoint.Port);

            UDPClient.Client.ReceiveTimeout = timeout;

            return Receive() != null;
        }
    }
}