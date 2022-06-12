using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Win_Dev.Data.Interfaces;

namespace Win_Dev.Data
{
    public class DatabaseWorker : IDatabaseService
    {
        public DataAccessObject DataAccessObject { get; set; }

        public System.Timers.Timer updateTimer;

        public event Action<bool> StatusChangedEvent;
        public event Action<bool> TryUpdateEvent;
        public event Action UpdatedDataLoadedEvent;

        private bool _isConnectionEstablished;
        public bool IsConnectionEstablished
        {
            get => _isConnectionEstablished;
            private set
            {

                if ((StatusChangedEvent != null)&&(value != _isConnectionEstablished)) StatusChangedEvent.Invoke(value);
                _isConnectionEstablished = value;
                
            }
        }

        public DatabaseWorker()
        {
                            
        }


        public void DatabaseWorkerInit(DataAccessObject newDataAccessObject)
        {
            DataAccessObject = (DataAccessObject == null) ? newDataAccessObject 
                                                          : DataAccessObject;

            IsConnectionEstablished = false;
        }

        public void ConnectionInit()
        {

            Task.Factory.StartNew(() =>
            {
                try
                {
                    
                    updateTimer = new System.Timers.Timer(5000);
                    updateTimer.AutoReset = true;
                    updateTimer.Elapsed += delegate
                    {
                        throw new ArgumentException();
                    };
                  
                    using (WinTaskContext wtContext = new WinTaskContext())
                    {
                        wtContext.Database.Connection.Open();
                        wtContext.Database.CreateIfNotExists();

                        DataAccessObject.UpdateContextInRepositories();
                        DataAccessObject.UpdateEntityModel();

                        IsConnectionEstablished = true;                       
                    };

                    updateTimer.Stop();

                    CreateContiniousUpdatingTask();
                }
                catch (ArgumentException)
                {
                    if (StatusChangedEvent != null) StatusChangedEvent.Invoke(IsConnectionEstablished);
                    if (TryUpdateEvent != null) TryUpdateEvent.Invoke(IsConnectionEstablished);
                }
                catch (Exception)
                {
                    IsConnectionEstablished = false;
                }
            });
        }


        /// <summary>
        /// Creates task which every 10 sec task calls data from the database.
        /// </summary>
        public void CreateContiniousUpdatingTask()
        {
            // Every 10 sec task calls data from db 

            updateTimer = new System.Timers.Timer(10000);
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += UpdateTimerElapsedAsync;
            updateTimer.Start();
        }

        /// <summary>
        /// Elapse timer connection check event handler.
        /// </summary>
        private void UpdateTimerElapsedAsync(object sender, ElapsedEventArgs e)
        {          
            Task.Factory.StartNew(() =>
            {
                if (TryUpdateEvent != null) TryUpdateEvent.Invoke(IsConnectionEstablished);

                try
                {
                    System.Timers.Timer timerShort = new System.Timers.Timer(2000);
                    timerShort.Elapsed += delegate
                    {
                        throw new InvalidOperationException();
                    };


                    DataAccessObject.UpdateEntityModel();

                    IsConnectionEstablished = true;

                    timerShort.Dispose();

                    if (UpdatedDataLoadedEvent != null) UpdatedDataLoadedEvent.Invoke();

                }
                catch (Exception ex)
                {

                    IsConnectionEstablished = false;
                    updateTimer.Stop();
                    updateTimer.Start();

                }
            });
        }

        private void ApplicationCloseRequested()
        {
            updateTimer.Dispose();
        }

        public void SaveChanges()
        {
            using (WinTaskContext wtContext = new WinTaskContext())
            {
                wtContext.SaveChanges();
            }
        }
    }
}
