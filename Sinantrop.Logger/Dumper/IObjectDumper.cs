using System.IO;

namespace Sinantrop.Logger.Dumper
{
    interface IObjectDumper
    {
        DumperConfiguration Configuration { get; set; }
        void Dump(object obj, TextWriter textWriter);
    }
}
