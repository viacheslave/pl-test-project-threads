using System;

namespace Plarium.Test.FourThreads.Workers
{
    // Base class for all the workers
    internal abstract class BaseWorker
    {
        protected BaseWorkerParameters Parameters { get; set; }
        
        // Can notify UI
        public event EventHandler<NotificationEventArgs> Notification;

        protected BaseWorker(BaseWorkerParameters workerParameters)
        {
            Parameters = workerParameters;        
        }

        // When a worker task starts processing, notifies UI
        // STARTED: <Worker Name>
        public virtual void Process()
        {
            NotifyStarted();
        }

        // In case of an exception, builds exceptions args and notifies UI
        protected void NotifyExceptionOccured(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            Notify(string.Format("{0}: {1}", GetType().Name, exception.Message));
        }

        // Creates "STARTED" UI notification
        protected void NotifyStarted()
        {
            Notify(BuildNotificationMessage("Started"));
        }

        // Creates "CANCELLED" UI notification
        protected void NotifyCancelled()
        {
            Notify(BuildNotificationMessage("Cancelled"));
        }

        // Creates "COMPLETED" UI notification
        protected void NotifyCompleted()
        {
            Notify(BuildNotificationMessage("Completed"));
        }

        protected void Notify(string message)
        {
            NotificationEventArgs notificationEventArgs = new NotificationEventArgs { Item = message };
            OnNotificationOccured(notificationEventArgs);
        }

        private void OnNotificationOccured(NotificationEventArgs notificationEventArgs)
        {
            if (Notification != null)
            {
                Notification(this, notificationEventArgs);
            }
        }

        private string BuildNotificationMessage(string prefix)
        {
            return string.Format("{0}: {1}", prefix.ToUpper(), GetType().Name);
        }
    }
}