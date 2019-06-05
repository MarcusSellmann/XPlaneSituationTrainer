namespace XPlaneSituationTrainer.Lib.Data {
    public interface ICommand {
        string DataRef { get; }
        string Value { get; }
    }
}