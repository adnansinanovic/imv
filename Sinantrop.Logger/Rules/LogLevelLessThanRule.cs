namespace Sinantrop.Logger.Rules
{
    public class LogLevelLessThanRule : IRule
    {
        public LogLevel LogLevel { get; set; }
        public bool IsValid(LogEvent logEvent)
        {
            return logEvent.Level < LogLevel;
        }
    }
}
