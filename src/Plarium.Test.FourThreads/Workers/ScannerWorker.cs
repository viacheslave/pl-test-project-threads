using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Plarium.Test.FourThreads.Workers
{
    // Concrete worker
    // Scans the folder provided
    internal class ScannerWorker : BaseWorker
    {
        // User-provided folder to scan
        private DirectoryInfo _directoryInfo;

        // Raised where new item is read
        // Subscribers: Tree and XML workers
        public event EventHandler<FileSystemInfoEventArgs> NewItemArrived;
        
        // Raised where no new items, scan is completed
        // Subscribers: Tree and XML workers
        public event EventHandler ScanCompleted;

        public ScannerWorker(BaseWorkerParameters workerParameters) : base(workerParameters)
        {
        }

        public override void Process()
        {
            base.Process();
            
            string path = ((ScannerWorkerParameters) Parameters).ScanFolder;
            _directoryInfo = new DirectoryInfo(path);

            try
            {
                ScanFolder(_directoryInfo);

                // Check if should be cancelled
                if (Parameters.CancellationToken.IsCancellationRequested)
                {
                    NotifyCancelled();
                    return;
                }

                NotifyCompleted();
            }
            finally
            {
                // Notify anayway
                // Other workers should not run any more
                NotifyScanCompleted();
            }
        }

        // Recursively scans folders and files
        private void ScanFolder(DirectoryInfo directoryInfo)
        {
            if (Parameters.CancellationToken.IsCancellationRequested)
            {
                return;
            }
            
            if (directoryInfo == null)
            {
                return;
            }

            if (directoryInfo != _directoryInfo)
            {
                NotifyNewItemArrived(directoryInfo);
            }

            EnumerateDirectories(directoryInfo);

            EnumerateFiles(directoryInfo);
        }

        // Wraps the "Enumerate" actions, handles exceptions and passes them to UI
        private void EnumerateSafely(Action action)
        {
            try
            {
                if (action != null)
                {
                    action.Invoke();
                }
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                NotifyExceptionOccured(unauthorizedAccessException);
            }
            catch (SecurityException securityException)
            {
                NotifyExceptionOccured(securityException);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                NotifyExceptionOccured(directoryNotFoundException);
            }
        }

        // Enumerates folders
        private void EnumerateDirectories(DirectoryInfo directoryInfo)
        {
            EnumerateSafely(() =>
            {
                IEnumerable<DirectoryInfo> directoryInfos = directoryInfo.EnumerateDirectories();
                foreach (DirectoryInfo folderInfo in directoryInfos)
                {
                    ScanFolder(folderInfo);
                }
            });
        }

        // Enumerates files
        private void EnumerateFiles(DirectoryInfo directoryInfo)
        {
            EnumerateSafely(() =>
            {
                IEnumerable<FileInfo> fileInfos = directoryInfo.EnumerateFiles();
                foreach (FileInfo fileInfo in fileInfos)
                {
                    // Check if should be cancelled
                    if (Parameters.CancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    
                    NotifyNewItemArrived(fileInfo);
                }
            });
        }

        private void NotifyNewItemArrived(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo == null)
            {
                return;
            }

            FileSystemInfoEventArgs fileSystemInfoEventArgs = new FileSystemInfoEventArgs();
            fileSystemInfoEventArgs.Item = fileSystemInfo;

            OnNewItemArrived(fileSystemInfoEventArgs);
        }

        private void NotifyScanCompleted()
        {
            OnScanCompleted();
        }

        protected virtual void OnNewItemArrived(FileSystemInfoEventArgs fileSystemInfoEventArgs)
        {
            if (NewItemArrived != null)
            {
                NewItemArrived(this, fileSystemInfoEventArgs);
            }
        }

        protected virtual void OnScanCompleted()
        {
            if (ScanCompleted != null)
            {
                ScanCompleted(this, EventArgs.Empty);
            }
        }

        // Default scan folder
        public static string DefaultScanFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Windows); }
        }
    }
}