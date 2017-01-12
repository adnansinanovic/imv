using System;
using System.Text;

namespace Sinantrop.Helper
{
    public static class ExeptionExtension
    {
        public static string GetString(this Exception source)
        {
            StringBuilder sb = new StringBuilder();
            while (source != null)
            {
                                
                sb.AppendLine(source.Message);                
                sb.AppendLine(source.StackTrace);
                sb.AppendLine();
                
                source = source.InnerException;
            }
            
            return sb.ToString();
        }
    }
}
