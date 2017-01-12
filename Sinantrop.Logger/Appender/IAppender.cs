using System.Collections.Generic;
using Sinantrop.Logger.Layout;
using Sinantrop.Logger.Rules;

namespace Sinantrop.Logger.Appender
{
    public interface IAppender
    {
        List<IRule> Rules { get; set; }
        string Name { get; set; }
        ILayout Layout { get; set; }
        void Enqueue(LogEvent logEvent);
        bool UseSeparateThread { get; set; }
    }
}
