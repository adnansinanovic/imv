using System;

namespace Sinantrop.Logger.Dumper.Formatter
{
    public interface IDumpFormatter
    {
        Type FormatterType { get; }
        string Format(object value, int intendation = 0);
    }
}
