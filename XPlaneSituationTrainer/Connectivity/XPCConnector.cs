using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneSituationTrainer.Lib.Connectivity {
    public class XPCConnector {
        #region Attributes
        private static XPCConnector _instance;
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

        public UdpClient Client { get; private set; }
        public bool Connected { get; private set; }
        #endregion

        private XPCConnector() {
            Client = new UdpClient();
        }

        public void ConnectTo(string hostname, int port) {
            _instance.Client.Connect(hostname, port);
            Connected = true;
        }

        public void Send(string message) {
            Send(Encoding.UTF8.GetBytes(message));
        }

        public void Send(byte[] message) {
            if (Connected) {
                Client.Send(message, message.Length);
            }
        }

        public async Task<XPCUdpMsgReceived> Receive() {
            var result = await Client.ReceiveAsync();
            return new XPCUdpMsgReceived {
                Message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
                Sender = result.RemoteEndPoint
            };
        }
    }
}