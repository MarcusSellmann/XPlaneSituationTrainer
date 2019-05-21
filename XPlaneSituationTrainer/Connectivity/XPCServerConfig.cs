namespace XPlaneSituationTrainer.Lib.Connectivity {
    public class XPCServerConfig {
        public string Hostname;
        public int Port;

        public XPCServerConfig() {
            Hostname = "127.0.0.1";
            Port = 43501;
        }

        public XPCServerConfig(string hostname, int port) {
            Hostname = hostname;
            Port = port;
        }
    }
}
