using System.Collections.Generic;
using XPC.DataRef.v1300.FlightModel;
using XPlaneSituationTrainer.Lib.Data;

namespace XPlaneSituationTrainer.Lib.Commanding.Conversion {
    public static class SituationToCommandConverter {
        public static List<ISituation> FlightModelPositionToCommands(FlightModelPosition flightModelPosition) {
            if (flightModelPosition == null) {
                throw new System.ArgumentNullException(nameof(flightModelPosition));
            }

            List<ISituation> commandList = new List<ISituation> {
                new FlightModelPositionSituation()
            };

            return commandList;
        }
    }
}
