using System;
using System.Globalization;

namespace Sinantrop.Logger.Pattern
{
    public class CurrentDateTimePattern : IPattern
    {
        public string Format { get; set; } = "yyyy-MM-dd HH-mm-ss:fff";

        public string Evaluate(LogEvent logEvent)
        {
            if (string.IsNullOrWhiteSpace(Format))
                return DateTime.Now.ToString(CultureInfo.InvariantCulture);

            return DateTime.Now.ToString(Format);
        }
    }
}
