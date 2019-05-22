namespace XPC.DataRef.v1300.FlightModel {
    class Controls {
        private static readonly string CONTROLS_BASE = "sim/flightmodel/controls";

        /// <summary>
        /// Actual speed brake deployment [0.0f..1.0f = schedule for in-air, 1.0f..1.5f = extra deployment when on ground]
        /// </summary>
        private static readonly string SBRK_RAT = "/sbrkrat";

        /// <summary>
        /// Requested flap deployment, 0.0f = off, 1.0f = max
        /// </summary>
        private static readonly string FLAP_RQST = "/flaprqst";

        /// <summary>
        /// Current Aileron Trim, -1.0f = max left, 1.0f = max right
        /// </summary>
        private static readonly string AIL_TRIM = "/ail_trim";

        /// <summary>
        /// Elevation Trim, -1.0f = max nose down, 1.0f = max nose up
        /// </summary>
        private static readonly string ELV_TRIM = "/elv_trim";

        /// <summary>
        /// Speed Brake, -0.5f = armed, 0.0f = off, 1.0f = max deployment
        /// </summary>
        private static readonly string SBRK_RQST = "/sbrkrqst";

        /// <summary>
        /// Requested thrust vector [0.0f..1.0f]
        /// </summary>
        private static readonly string VEC_THR_RQST = "/vectrqst";

        /// <summary>
        /// Parking Brake, 1.0f = max
        /// </summary>
        private static readonly string PARKBRAKE = "/parkbrake";

        /// <summary>
        /// Rudder Trim, -1.0f = max left, 1.0f = max right
        /// </summary>
        private static readonly string RUD_TRIM = "/rud_trim";

        /// <summary>
        /// Requested incidence [0.0f..1.0f]
        /// </summary>
        private static readonly string INCID_RQST = "/incid_rqst";

        /// <summary>
        /// Requested dihedral [0.0f..1.0f]
        /// </summary>
        private static readonly string DIHED_RQST = "/dihed_rqst";

        /// <summary>
        /// Actual thrust vector [0.0f..1.0f]
        /// </summary>
        private static readonly string VEC_THR_RAT = "/vect_rat";

        /// <summary>
        /// Actual incidence [0.0f..1.0f]
        /// </summary>
        private static readonly string INCID_RAT = "/incid_rat";

        /// <summary>
        /// Actual dihedral [0.0f..1.0f]
        /// </summary>
        private static readonly string DIHED_RAT = "/dihed_rat";
    }

    class Position {
        private static readonly string POSITION_BASE = "sim/flightmodel/position";

        /// <summary>
        /// The location of the plane in OpenGL coordinates [meters][double]
        /// </summary>
        public static readonly string LOCAL_X = POSITION_BASE + "/local_x";

        /// <summary>
        /// The location of the plane in OpenGL coordinates [meters][double]
        /// </summary>
        public static readonly string LOCAL_Y = POSITION_BASE + "/local_Y";

        /// <summary>
        /// The location of the plane in OpenGL coordinates [meters][double]
        /// </summary>
        public static readonly string LOCAL_Z = POSITION_BASE + "/local_z";

        /// <summary>
        /// The pitch relative to the plane normal to the Y axis in degrees - OpenGL coordinates [degrees][float]
        /// </summary>
        public static readonly string THETA = POSITION_BASE + "/theta";

        /// <summary>
        /// The roll of the aircraft in degrees - OpenGL coordinates [degrees][float]
        /// </summary>
        public static readonly string PHI = POSITION_BASE + "/phi";

        /// <summary>
        /// The true heading of the aircraft in degrees from the Z axis - OpenGL coordinates [degrees][float]
        /// </summary>
        public static readonly string PSI = POSITION_BASE + "/psi";

        /// <summary>
        /// The velocity in local OGL coordinates [mtr/sec][float]
        /// </summary>
        public static readonly string LOCAL_VX = POSITION_BASE + "/local_vx";

        /// <summary>
        /// The velocity in local OGL coordinates [mtr/sec][float]
        /// </summary>
        public static readonly string LOCAL_VY = POSITION_BASE + "/local_vy";

        /// <summary>
        /// The velocity in local OGL coordinates [mtr/sec][float]
        /// </summary>
        public static readonly string LOCAL_VZ = POSITION_BASE + "/local_vz";

        /// <summary>
        /// The acceleration in local OGL coordinates [mtr/sec2][float]
        /// </summary>
        public static readonly string LOCAL_AX = POSITION_BASE + "/local_ax";

        /// <summary>
        /// The acceleration in local OGL coordinates [mtr/sec2][float]
        /// </summary>
        public static readonly string LOCAL_AY = POSITION_BASE + "/local_ay";

        /// <summary>
        /// The acceleration in local OGL coordinates [mtr/sec2][float]
        /// </summary>
        public static readonly string LOCAL_AZ = POSITION_BASE + "/local_az";

        /// <summary>
        /// The roll rotation rates (relative to the flight) [rad/sec][float]
        /// </summary>
        public static readonly string P_RAD = POSITION_BASE + "/Prad";

        /// <summary>
        /// The pitch rotation rates (relative to the flight) [rad/sec][float]
        /// </summary>
        public static readonly string Q_RAD = POSITION_BASE + "/Qrad";

        /// <summary>
        /// The yaw rotation rates (relative to the flight) [rad/sec][float]
        /// </summary>
        public static readonly string R_RAD = POSITION_BASE + "/Rrad";

        /// <summary>
        /// VVI (vertical velocity in feet per second) [feet/min][float]
        /// </summary>
        public static readonly string VH_IND_FPM = POSITION_BASE + "/vh_ind_fpm";
    }
}