using XPlaneSituationTrainer.Lib.Connectivity;

namespace XPlaneSituationTrainer.Lib {
    public interface IXPCController {
        /// <summary>
        /// Gets the server configuration.
        /// </summary>
        /// <value>The server configuration containing IP and port of the XPC server.</value>
        XPCServerConfig ServerConfig { get; }

        /// <summary>
        /// Gets the UDP client.
        /// </summary>
        /// <value>The UDP client pointing at the XPC server.</value>
        XPCConnector UdpClient { get; }

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
        /// <returns><c>true</c>, if the server has been connected successfully, <c>false</c> otherwise.</returns>
        bool ConnectToServer();
    }
}
