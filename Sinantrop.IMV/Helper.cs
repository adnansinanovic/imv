using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Sinantrop.IMV
{
    internal static class Helper
    {
  

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(unixTimeStamp).ToLocalTime();
            return dt;
        }

        public static double  DateTimeToUnixTimestamp(DateTime dateTime)
        {
            double result =  (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

            if (result < 0)
                result = 0;

            return result;
        }


        public static string ExceptionToString(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.Message);

            string stackTrace = ex.StackTrace;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;

                sb.AppendLine(ex.Message);
                sb.AppendLine();
            }

            sb.AppendLine(stackTrace);

            return sb.ToString();
        }
    }
}
