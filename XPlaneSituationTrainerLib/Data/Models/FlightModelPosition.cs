namespace XPlaneSituationTrainer.Lib.Data {
    public class FlightModelPosition {
        #region Properties
        /// <summary>
        /// The latitude of the aircraft.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; private set; }

        /// <summary>
        /// The longitude of the aircraft.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; private set; }

        /// <summary>
        /// The elevation above MSL of the aircraft.
        /// </summary>
        /// <value>The elevation.</value>
        public int Elevation { get; private set; }

        /// <summary>
        /// The pitch relative to the plane normal to the Y axis in degrees.
        /// </summary>
        /// <value>The pitch.</value>
        public float Pitch { get; private set; }

        /// <summary>
        /// The roll of the aircraft in degrees.
        /// </summary>
        /// <value>The roll.</value>
        public float Roll { get; private set; }

        /// <summary>
        /// The true heading of the aircraft in degrees from the Z axis.
        /// </summary>
        /// <value>The heading.</value>
        public float Heading { get; private set; }
        #endregion

        public FlightModelPosition() {
        }
    }
}
