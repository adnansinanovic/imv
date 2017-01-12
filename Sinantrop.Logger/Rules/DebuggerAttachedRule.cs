using System.Diagnostics;

namespace Sinantrop.Logger.Rules
{
    public class DebuggerAttachedRule : IRule
    {       
        public bool IsValid(LogEvent logEvent)
        {
            return Debugger.IsAttached;
        }
    }
}
