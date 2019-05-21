namespace XPC.DataRef.v1300.FlightModel {
    class Position {
        private static readonly string POSITION_BASE = "sim/flightmodel/position";

        /// <summary>
        /// The location of the plane in OpenGL coordinates
        /// </summary>
        public static readonly string LOCAL_X = POSITION_BASE + "/local_x";

        /// <summary>
        /// The location of the plane in OpenGL coordinates
        /// </summary>
        public static readonly string LOCAL_Y = POSITION_BASE + "/local_Y";

        /// <summary>
        /// The location of the plane in OpenGL coordinates
        /// </summary>
        public static readonly string LOCAL_Z = POSITION_BASE + "/local_z";

        /// <summary>
        /// The pitch relative to the plane normal to the Y axis in degrees - OpenGL coordinates
        /// </summary>
        public static readonly string THETA = POSITION_BASE + "/theta";

        /// <summary>
        /// The roll of the aircraft in degrees - OpenGL coordinates
        /// </summary>
        public static readonly string PHI = POSITION_BASE + "/phi";

        /// <summary>
        /// The true heading of the aircraft in degrees from the Z axis - OpenGL coordinates
        /// </summary>
        public static readonly string PSI = POSITION_BASE + "/psi";

        /// <summary>
        /// The velocity in local OGL coordinates
        /// </summary>
        public static readonly string LOCAL_VX = POSITION_BASE + "/local_vx";

        /// <summary>
        /// The velocity in local OGL coordinates
        /// </summary>
        public static readonly string LOCAL_VY = POSITION_BASE + "/local_vy";

        /// <summary>
        /// The velocity in local OGL coordinates
        /// </summary>
        public static readonly string LOCAL_VZ = POSITION_BASE + "/local_vz";

        /// <summary>
        /// The acceleration in local OGL coordinates
        /// </summary>
        public static readonly string LOCAL_AX = POSITION_BASE + "/local_ax";

        /// <summary>
        /// The acceleration in local OGL coordinates
        /// </summary>
        public static readonly string LOCAL_AY = POSITION_BASE + "/local_ay";

        /// <summary>
        /// The acceleration in local OGL coordinates
        /// </summary>
        public static readonly string LOCAL_AZ = POSITION_BASE + "/local_az";

        /// <summary>
        /// The roll rotation rates (relative to the flight) [rad/sec]
        /// </summary>
        public static readonly string P_RAD = POSITION_BASE + "/Prad";

        /// <summary>
        /// The pitch rotation rates (relative to the flight)
        /// </summary>
        public static readonly string Q_RAD = POSITION_BASE + "/Qrad";

        /// <summary>
        /// The yaw rotation rates (relative to the flight)
        /// </summary>
        public static readonly string R_RAD = POSITION_BASE + "/Rrad";

        /// <summary>
        /// VVI (vertical velocity in feet per second)
        /// </summary>
        public static readonly string VH_IND_FPM = POSITION_BASE + "/vh_ind_fpm";
    }
}