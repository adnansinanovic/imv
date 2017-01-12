namespace Sinantrop.Logger.Rules
{
    public class LogLevelEqualRule : IRule
    {
        public LogLevel LogLevel { get; set; }
        public bool IsValid(LogEvent logEvent)
        {
            return logEvent.Level == LogLevel;
        }
    }
}
