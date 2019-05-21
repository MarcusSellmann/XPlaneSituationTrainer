using System.Collections.Generic;

namespace XPlaneSituationTrainer.Lib.Data {
    public interface ISituation {
        List<ICommand> Commands { get; }

        void SendCommandsToXPlane();
    }
}
