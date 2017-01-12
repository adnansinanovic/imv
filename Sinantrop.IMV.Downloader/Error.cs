using System;
using System.Windows.Forms;
using Sinantrop.Helper;
using Sinantrop.Logger;

namespace Sinantrop.IMV.Downloader
{
    class Error
    {
        public static void Write(Exception exception)
        {
            StaticLogger.WriteLine(exception.GetString());

            MessageBox.Show(@"Unexpected error. For more info check log file.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
