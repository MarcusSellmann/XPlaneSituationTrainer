using System.IO;
using System.Xml.Serialization;

using XPlaneSituationTrainer.Lib.Connectivity;

namespace XPlaneSituationTrainer.Lib {
    public class XPCController : IXPCController {
        public XPCServerConfig ServerConfig { get; private set; }
        public XPCConnector UdpClient { get; private set; }

        public bool ConnectToServer() {
            UdpClient = XPCConnector.ConnectTo(ServerConfig.Hostname, ServerConfig.Port);

            return UdpClient != null; 
        }

        public void ReadServerConfigFromXml() {
            XmlSerializer reader = new XmlSerializer(ServerConfig.GetType());
            StreamReader file = new StreamReader("server.config");

            ServerConfig = reader.Deserialize(file) as XPCServerConfig;
        }

        public void SaveServerConfigToXml() {
            XmlSerializer writer = new XmlSerializer(ServerConfig.GetType());
            StreamWriter file = new StreamWriter("server.config");
            writer.Serialize(file, ServerConfig);
            file.Close();
        }
    }
}





class Program {
    static void Main(string[] args) {
        //wait for reply messages from server and send them to console 
        Task.Factory.StartNew(async () => {
            while (true) {
                try {
                    var received = await client.Receive();
                    Console.WriteLine(received.Message);
                    if (received.Message.Contains("quit"))
                        break;
                } catch (Exception ex) {
                    Debug.Write(ex);
                }
            }
        });

        //type ahead :-)
        string read;
        do {
            read = Console.ReadLine();
            client.Send(read);
        } while (read != "quit");
    }
}