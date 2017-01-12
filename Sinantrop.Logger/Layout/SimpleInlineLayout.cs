using System.IO;
using Sinantrop.Logger.Dumper;
using Sinantrop.Logger.Pattern;

namespace Sinantrop.Logger.Layout
{
    public class SimpleInlineLayout : ILayout
    {
        protected readonly TraversalDumper Dumper = new TraversalDumper();
        public IPattern Header { get; set; } = new CurrentDateTimePattern();
        public IPattern Footer { get; set; }

        public string HeaderDelimiter { get; set; } = " > ";

        public string FooterDelimiter { get; set; } = " :: ";
        public virtual void Format(TextWriter writer, LogEvent logEvent)
        {
            WriteHeader(writer, logEvent);

            WriteBody(writer, logEvent);

            WriteFooter(writer, logEvent);
        }

        private void WriteBody(TextWriter writer, LogEvent logEvent)
        {
            object message = logEvent.GetLogObject();
            Dumper.Dump(message, writer);
            writer.Write(string.Empty);
        }

        protected virtual void WriteFooter(TextWriter writer, LogEvent logEvent)
        {
            var footer = Footer?.Evaluate(logEvent);

            if (!string.IsNullOrWhiteSpace(FooterDelimiter) && !string.IsNullOrWhiteSpace(footer))
                footer = $"{footer}{HeaderDelimiter}";

            if (!string.IsNullOrWhiteSpace(footer))
                writer.WriteLine(footer);
            else
                writer.WriteLine();
        }
        protected virtual void WriteHeader(TextWriter writer, LogEvent logEvent)
        {            
            var header = Header?.Evaluate(logEvent);

            if (!string.IsNullOrWhiteSpace(HeaderDelimiter) && !string.IsNullOrWhiteSpace(header))
                header = $"{header}{HeaderDelimiter}";

            if (!string.IsNullOrWhiteSpace(header))
                writer.Write(header);
        }
    }
}
