namespace Sinantrop.Logger.Pattern
{
    public class DefaultHeaderPattern : IPattern
    {
        public string Format { get; set; } = "yyyy-MM-dd HH-mm-ss:fff";

        public string Evaluate(LogEvent logEvent)
        {
            return $"----- { logEvent.Created.ToString(Format) } -- Thread: {logEvent.ManagedThreadId} :: {logEvent.ThreadName ?? "[]"}--------------------";
        }
    }
}
