using System.Collections.Generic;
using XPC.DataRef.v1300.FlightModel;
using XPlaneSituationTrainer.Lib.Data;

namespace XPlaneSituationTrainer.Lib.Commanding.Conversion {
    public static class SituationToCommandConverter {
        public static List<ICommand> FlightModelPositionToCommands(FlightModelPosition flightModelPosition) {
            if (flightModelPosition == null) {
                throw new System.ArgumentNullException(nameof(flightModelPosition));
            }

            List<ICommand> commandList = new List<ICommand> {
                new FlightModelPositionCommand(Position.LOCAL_X, flightModelPosition.Latitude.ToString())
            };

            return commandList;
        }
    }
}
