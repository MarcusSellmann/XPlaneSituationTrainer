using XPlaneSituationTrainer.Lib.Data;

namespace XPlaneSituationTrainerLib.Data.Models {
    public class Command : ICommand {
        public string DataRef { get; private set; }
        public string Value { get; private set; }

        public Command(string dataRef, string value) {
            DataRef = dataRef;
            Value = value;
        }

        override
        public string ToString() => $"{DataRef} {Value}";
    }
}
