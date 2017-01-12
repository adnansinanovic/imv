using System;
using System.Data.SQLite;

namespace Sinantrop.IMV.Db
{
    [SQLiteFunction(Name = "REGEXP", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class Regexp : SQLiteFunction
    {
        /// <summary>
        /// How to use:
        /// SELECT * FROM TableA where ColumnA REGEXP <REGEX_HERE>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object Invoke(object[] args)
        {
            //string command = args[0].ToString();
            //string content = args[1].ToString();                        

            return System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(args[1]), Convert.ToString(args[0]));            
        }
    }
}
