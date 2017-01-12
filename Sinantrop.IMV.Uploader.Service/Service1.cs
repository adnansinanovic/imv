using System;
using System.ServiceProcess;
using Sinantrop.Helper;
using Sinantrop.IMV.Sync;

namespace Sinantrop.IMV.Uploader.Service
{
    public partial class Service1 : ServiceBase
    {
        Server _server;
        static void Main(string[] args)
        {
            using (Service1 service = new Service1())
            {
                try
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[] { service };

                    if (!Environment.UserInteractive)
                    {
                        Run(ServicesToRun);
                    }
                    else
                    {
                        if (args != null && args.Length == 1 && args[0] == "baboruco")
                        {

                            service.OnStart(args);
                            Console.ReadKey();

                            service.OnStop();
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Error I am");
                            Console.ReadKey();
                        }
                    }
                }
                catch (Exception ex)
                {
                    UploaderLogger.Instance.WriteException(ex, MessengerType.Unknown);                    
                    Console.ReadKey();
                }
            }
        }

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {            
            _server = new Server();
            _server.Start();
        }

        protected override void OnStop()
        {         
            _server.Stop();
        }
    }
}
