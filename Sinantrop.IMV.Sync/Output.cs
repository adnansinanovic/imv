using System;


namespace Sinantrop.IMV.Sync
{
    public static class Output1
    {
        public static void WriteInfo(int level, string msg)
        {
            WriteLine(level, msg, ConsoleColor.Gray);
        }

        public static void WriteError(int level, string msg)
        {
            WriteLine(level, msg, ConsoleColor.Red);
        }

        public static void WriteLine(int level, string msg, ConsoleColor color)
        {
            msg = $"{string.Empty.PadRight(level, ' ')}{msg}";

            if (Environment.UserInteractive)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
            }

            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}
