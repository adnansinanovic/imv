namespace Sinantrop.Logger.Pattern
{
    public class ValuePattern<T> : IPattern
    {
        public ValuePattern()
        {
        }

        public ValuePattern(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
        public string Evaluate(LogEvent logEvent)
        {
            return Value as string ?? Value?.ToString() ?? string.Empty;
        }
    }
}
