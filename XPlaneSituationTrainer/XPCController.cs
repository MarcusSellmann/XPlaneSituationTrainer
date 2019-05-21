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
