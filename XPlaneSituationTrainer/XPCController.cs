using System.IO;
using System.Xml.Serialization;

using XPlaneSituationTrainer.Lib.Commanding;
using XPlaneSituationTrainer.Lib.Connectivity;

namespace XPlaneSituationTrainer.Lib {
    public class XPCController : IXPCController {
        #region Attributes
        private static IXPCController _instance;
        private XPCDirector _director;
        #endregion

        #region Properties
        public IXPCController Instance {
            get {
                if (_instance == null) {
                    _instance = new XPCController();
                }

                return _instance;
            }
        }

        public XPCDirector SituationDirector { get; private set; }

        public XPCServerConfig ServerConfig { get; private set; }
        #endregion

        public void ConnectToServer() {
            XPCConnector.Instance.ConnectTo(ServerConfig.Hostname, ServerConfig.Port);
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

        private XPCController() {
            _director = new XPCDirector();
        }
    }
}