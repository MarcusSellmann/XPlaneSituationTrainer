namespace XPlaneSituationTrainer.Lib.Data {
    public class FlightModelPositionCommand : ICommand {
        public string DataRef { get; private set; }
        public string Value { get; private set; }

        public FlightModelPositionCommand(string dataRef, string value) {
            DataRef = dataRef;
            Value = value;
        }

        override
        public string ToString() => $"{DataRef} {Value}";
    }
}
