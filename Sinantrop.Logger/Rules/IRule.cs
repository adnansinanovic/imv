namespace Sinantrop.Logger.Rules
{
    public interface IRule
    {
        bool IsValid(LogEvent logEvent);
    }
}
