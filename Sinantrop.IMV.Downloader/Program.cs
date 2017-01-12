using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Sinantrop.Db.SQLite;
using Sinantrop.Db.SQLite.TableCreators;
using Sinantrop.DB.SQLite;
using Sinantrop.Helper;
using Sinantrop.IMV.Sync;
using Sinantrop.Logger;
using Sinantrop.Logger.Appender;

namespace Sinantrop.IMV.Downloader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] arg)
        {
            try
            {
                InitLogger();
                InitDb();               
                InjectOauth(arg);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                Error.Write(ex);
            }
        }

        private static void InjectOauth(string[] arg)
        {
            if (arg != null && arg.Length > 0)
            {                
                DropboxOAuthProvider.Inject(arg[0]);
            }
        }

        private static void InitLogger()
        {
            StaticLogger.Configuration.AddAppender(new FileAppender());

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void InitDb()
        {
            OrmEnvironment.Instance.AddContext(new OrmContextSettings()
            {
                DbName = "OnlineData.db3",
                TableCreators = new List<ITableCreator>()
                {
                    new FromAssembly(Assembly.GetAssembly(typeof(Uploader)))
                }
            });
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            Error.Write(ex);

            Environment.Exit(1);
        }
    }
}
