using System.Net;

namespace XPlaneSituationTrainer.Lib.Connectivity {
    public struct XPCUdpMsgReceived {
        public IPEndPoint Sender;
        public string Message;
    }
}
