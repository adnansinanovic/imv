using System.IO;
using Sinantrop.Logger.Pattern;

namespace Sinantrop.Logger.Layout
{
    public interface ILayout
    {
        void Format(TextWriter writer, LogEvent logEvent);
        IPattern Header { get; set; }        
        IPattern Footer { get; set; }
    }
}
