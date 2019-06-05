using XPlaneSituationTrainer.Lib.Commanding;
using XPlaneSituationTrainer.Lib.Connectivity;

namespace XPlaneSituationTrainer.Lib {
    public interface IXPCController {
        /// <summary>
        /// Gets the server configuration.
        /// </summary>
        XPCServerConfig ServerConfig { get; }

        /// <summary>
        /// The director which coordinates the situations created by the user.
        /// </summary>
        XPCDirector SituationDirector { get; }

        /// <summary>
        /// Saves the server configuration to an xml file.
        /// </summary>
        void SaveServerConfigToXml();

        /// <summary>
        /// Reads the server configuration from an xml file.
        /// </summary>
        void ReadServerConfigFromXml();

        /// <summary>
        /// Connects to XPC server using the specified IP and port from the <c>ServerConfig</c>.
        /// </summary>
        void ConnectToServer();
    }
}
