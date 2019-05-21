using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneSituationTrainer.Lib.Connectivity {
    public class XPCConnector {
        private readonly UdpClient Client;

        private XPCConnector() {
            Client = new UdpClient();
        }

        public static XPCConnector ConnectTo(string hostname, int port) {
            var connection = new XPCConnector();
            connection.Client.Connect(hostname, port);
            return connection;
        }

        public void Send(string message) {
            var datagram = Encoding.ASCII.GetBytes(message);
            Client.Send(datagram, datagram.Length);
        }

        public async Task<XPCUdpMsgReceived> Receive() {
            var result = await Client.ReceiveAsync();
            return new XPCUdpMsgReceived() {
                Message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
                Sender = result.RemoteEndPoint
            };
        }
    }
}