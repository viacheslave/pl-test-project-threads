using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace Plarium.Test.FourThreads.Workers
{
    // Base class for all queue-based workers, consumers of Scanner
    internal abstract class BaseQueueWorker : BaseWorker
    {
        // Indicates that the scan is completed
        // and the queue will not increase in size
        private long _isCompleted;

        // Main thread-safe FIFO items storage
        private readonly ConcurrentQueue<FileSystemInfo> _processingQueue = new ConcurrentQueue<FileSystemInfo>();

        // Raised when the queue size is likely changed, when item is dequeued
        public event EventHandler<QueueStatisticsEventArgs> StatisticsUpdated; 

        protected BaseQueueWorker(BaseWorkerParameters workerParameters) : base(workerParameters)
        {
        }
        
        // Invoked when new scanner has new item
        public void OnNewItemArrived(object sender, FileSystemInfoEventArgs e)
        {
            if (e != null && e.Item != null)
            {
                _processingQueue.Enqueue(e.Item);
            }
        }

        // Invoked when the scanning process is completed
        public virtual void OnScanCompleted(object sender, EventArgs e)
        {
            Interlocked.Increment(ref _isCompleted);
        }
        
        public override void Process()
        {
            base.Process();

            try
            {
                // When overriden - creates and initializes a new XMLWriter instance
                Prepare();
                
                while (true)
                {
                    // Check if should be cancelled
                    if (Parameters.CancellationToken.IsCancellationRequested)
                    {
                        NotifyCancelled();
                        break;
                    }

                    // Check and notify if should complete
                    long completed = Interlocked.Read(ref _isCompleted);
                    if (completed != 0 && _processingQueue.Count == 0)
                    {
                        NotifyCompleted();
                        break;
                    }
                    
                    // Tries to get new item for processing
                    // Updates UI for statistics
                    // Processes the item
                    FileSystemInfo fileSystemInfo;
                    if (_processingQueue.Count > 0 && _processingQueue.TryDequeue(out fileSystemInfo))
                    {
                        NotifyStatisticsUpdated(_processingQueue.Count);
                    
                        ProcessItem(fileSystemInfo);
                    }
                }
            }
            finally
            {
                // When overrided, flushes and closes the XMLWriter instance
                Dispose();
            }
        }

        protected virtual void Prepare()
        {
        }

        protected virtual void Dispose()
        {
        }

        protected abstract void ProcessItem(FileSystemInfo fileSystemInfo);

        // Gets parent folder for a FileSystemInfo item
        protected DirectoryInfo GetParentFolder(FileSystemInfo fileSystemInfo)
        {
            DirectoryInfo folderInfo = null;

            if (fileSystemInfo is FileInfo)
            {
                folderInfo = (fileSystemInfo as FileInfo).Directory;
            }

            if (fileSystemInfo is DirectoryInfo)
            {
                folderInfo = (fileSystemInfo as DirectoryInfo).Parent;
            }

            return folderInfo;
        }

        protected void NotifyStatisticsUpdated(long queueSize)
        {
            QueueStatisticsEventArgs queueStatisticsEventArgs = new QueueStatisticsEventArgs { QueueSize = queueSize };
            OnStatisticsUpdated(queueStatisticsEventArgs);
        }

        protected virtual void OnStatisticsUpdated(QueueStatisticsEventArgs queueStatisticsEventArgs)
        {
            if (StatisticsUpdated != null)
            {
                StatisticsUpdated(this, queueStatisticsEventArgs);
            }
        }
    }
}