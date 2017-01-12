using System;
using System.Collections.Generic;
using Sinantrop.Db.SQLite;
using Sinantrop.Db.SQLite.TableCreators;
using Sinantrop.DB.SQLite;
using SQLite;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sinantrop.IMV.Sync.Model;

namespace Sinantrop.IMV.Uploader.Service
{
    class Server
    {
        ConcurrentBag<Task> _tasks;
        CancellationTokenSource _cancellationTokenSource;

        public Server()
        {
            UploaderLogger.Instance.WriteLine("Initializing db...", 0);
            InitializeDb();

            UploaderLogger.Instance.WriteLine("Initializing app...", 0);
            InitializeApp();
        }

        private static void InitializeApp()
        {
            if (OrmEntity.Count<ComputerInfo>() == 0)
            {
                ComputerInfo info = new ComputerInfo
                {
                    MachineName = Environment.MachineName,
                    Guid = Guid.NewGuid().ToString(),
                    WindowsUserName = Environment.UserName,
                    WindowsVersion = Environment.OSVersion.Version.ToString()
                };
                OrmEntity.Save(info);
            }

            ComputerInfoHolder.Info = OrmEntity.First<ComputerInfo>();            
        }

        private void InitializeDb()
        {                        
            string dbname =  Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Uploader.db3");

            OrmEnvironment.Instance.AddContext(new OrmContextSettings()
            {
                DbName = dbname,
                OpenFlags = SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite,

                TableCreators = new List<ITableCreator>()
                {
                    new FromTypes(new List<Type>()
                    {
                        typeof(DownloadItem),
                        typeof(UploadItem),
                        typeof(ErrorItem),
                        typeof(ComputerInfo)
                    })
                }
            });
        }

        internal void Start()
        {
            _tasks = new ConcurrentBag<Task>();
            _cancellationTokenSource = new CancellationTokenSource();

            AddTask(ProcessSkype);
            AddTask(ProcessViber);
        }

        private void AddTask(Action action)
        {
            int wait = Config.GetTaskIntervalMiliseconds();
            _tasks.Add(PeriodicTaskFactory.Start(action, wait, 5000, synchronous: true, cancelToken: _cancellationTokenSource.Token));
        }

        private async void ProcessSkype()
        {
            UploaderLogger.Instance.WriteLine("Skype started....", 0);
            await Process(MessengerType.Skype);
            UploaderLogger.Instance.WriteLine("Skype Done....", 0);
        }

        private async void ProcessViber()
        {
            UploaderLogger.Instance.WriteLine("Viber started....", 0);
            await Process(MessengerType.Viber);
            UploaderLogger.Instance.WriteLine("Viber Done....", 0);
        }

        private async Task Process(MessengerType messengerType)
        {
            try
            {
                Processor processor = new Processor(messengerType);
                await processor.Process();
            }
            catch (Exception ex)
            {                
                UploaderLogger.Instance.WriteException(ex, messengerType);
            }
        }


        internal void Stop()
        {
            try
            {
                _cancellationTokenSource.Cancel();
                Task.WaitAll(_tasks.ToArray());
            }
            catch (AggregateException e)
            {
                UploaderLogger.Instance.WriteLine("\nAggregateException thrown with the following inner exceptions:", 0);
                foreach (var v in e.InnerExceptions)
                {
                    TaskCanceledException taskCanceledException = v as TaskCanceledException;
                    UploaderLogger.Instance.WriteLine(taskCanceledException != null ? $"   TaskCanceledException: Task {taskCanceledException.Task.Id}" : $"   Exception: {v.GetType().Name}", 1);
                }
                UploaderLogger.Instance.WriteLine(string.Empty);
            }
            finally
            {
                _cancellationTokenSource.Dispose();
            }
        }        
    }
}
