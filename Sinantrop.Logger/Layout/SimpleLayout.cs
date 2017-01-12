using System.IO;
using Sinantrop.Logger.Dumper;
using Sinantrop.Logger.Pattern;

namespace Sinantrop.Logger.Layout
{
    public class SimpleLayout : ILayout
    {
        protected readonly TraversalDumper Dumper = new TraversalDumper();
        public IPattern Header { get; set; } = new DefaultHeaderPattern();
        public IPattern Footer { get; set; }
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
            writer.WriteLine();
        }

        protected virtual void WriteFooter(TextWriter writer, LogEvent logEvent)
        {
            var footer = Footer?.Evaluate(logEvent);

            if (!string.IsNullOrWhiteSpace(footer))
                writer.WriteLine(footer);
        }
        protected virtual void WriteHeader(TextWriter writer, LogEvent logEvent)
        {
            var header = Header?.Evaluate(logEvent);

            if (!string.IsNullOrWhiteSpace(header))
                writer.WriteLine(header);
        }
    }
}
