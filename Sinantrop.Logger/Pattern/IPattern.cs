namespace Sinantrop.Logger.Pattern
{
    public interface IPattern
    {
        string Evaluate(LogEvent logEvent);
    }
}
