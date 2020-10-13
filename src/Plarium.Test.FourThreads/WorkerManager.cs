using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Plarium.Test.FourThreads.Extensions;
using Plarium.Test.FourThreads.Workers;

namespace Plarium.Test.FourThreads
{
    // Central point for creating and controlling worker threads
    internal class WorkerManager
    {
        private readonly MainWindow _mainWindow;
        
        private CancellationTokenSource _cancellationTokenSource;
        private Task[] _tasks;

        public WorkerManager(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        // Creates and starts new worker threads via tasks
        public void StartNew()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            // Input parameters for threads
            // Provides cancellation token, a folder to scan and an XML output file path
            XmlWorkerParameters workerParameters = 
                new XmlWorkerParameters()
                {
                    CancellationToken = _cancellationTokenSource.Token,
                    ScanFolder = _mainWindow.ControlTxtScanFolder.Text,
                    XmlFileLocation = _mainWindow.ControlTxtXmlOutputFilePath.Text,
                };

            var scannerWorker = new ScannerWorker(workerParameters);
            var treeWorker = new TreeQueueWorker(workerParameters);

            // Based on user input, it is possible to use Simple and Advanced XML Worker
            // Simple - no folder SIZEs in XML, linear writing
            // Advanced - with folder SIZEs, based on a custom tree structure and partial flush operations
            BaseQueueXmlWorker xmlWorker;
            if (_mainWindow.ControlChkUseAdvancedXmlWorker.Checked)
            {
                xmlWorker = new AdvancedQueueXmlWorker(workerParameters);
            }
            else
            {
                xmlWorker = new SimpleQueueXmlWorker(workerParameters);
            }

            scannerWorker.NewItemArrived += treeWorker.OnNewItemArrived;
            scannerWorker.NewItemArrived += xmlWorker.OnNewItemArrived;
            scannerWorker.ScanCompleted += treeWorker.OnScanCompleted;
            scannerWorker.ScanCompleted += xmlWorker.OnScanCompleted;
            scannerWorker.Notification += _mainWindow.WorkerOnNotification;

            treeWorker.TreeViewUpdate += _mainWindow.UpdateTreeView;
            treeWorker.Notification += _mainWindow.WorkerOnNotification;
            
            xmlWorker.Notification += _mainWindow.WorkerOnNotification;

            if (_mainWindow.ControlChkEnableLiveStatistics.Checked)
            {
                treeWorker.StatisticsUpdated += _mainWindow.TreeWorkerOnStatisticsUpdated;
                xmlWorker.StatisticsUpdated += _mainWindow.XmlWorkerOnStatisticsUpdated;
            }

            Task taskScannerWorker = new Task(scannerWorker.Process, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning);
            Task taskTreeWorker = new Task(treeWorker.Process, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning);
            Task taskXmlWorker = new Task(xmlWorker.Process, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning);

            // Task - Worker type map. Needed for UI notifications
            Dictionary<Task, string> tasksMap = new Dictionary<Task, string>()
            {
                {taskScannerWorker, scannerWorker.GetType().Name}, 
                {taskTreeWorker, treeWorker.GetType().Name}, 
                {taskXmlWorker, xmlWorker.GetType().Name}
            };

            AttachSingleContinuations(tasksMap, OnTaskFaulted, TaskContinuationOptions.OnlyOnFaulted);
            
            _tasks = tasksMap.Keys.ToArray();
            Task.Factory.ContinueWhenAll(_tasks, ContinuationActionWhenAll);

            _tasks.ToList().ForEach(_ => _.Start());
        }

        // Signals all workers to cancel
        public void StopWorkers()
        {
            _cancellationTokenSource.Cancel();
        }

        // When all tasks are in completed state, updates UI (enables controls)
        // or exits the application by calling Close on the Main Form
        private void ContinuationActionWhenAll(Task[] tasks)
        {
            if (_mainWindow.IsClosing)
            {
                _mainWindow.InvokeIfRequired(_mainWindow, () => _mainWindow.Close());
            }
            else
            {
                _mainWindow.PostProcessUI();
            }
        }

        // Attaches ContinueWith task for every worker task
        private void AttachSingleContinuations(Dictionary<Task, string> tasksMap, Action<Task, string> onTaskEvent, TaskContinuationOptions options)
        {
            Task[] tasks = tasksMap.Keys.ToArray();
            foreach (Task task in tasks)
            {
                task.ContinueWith((t) => onTaskEvent(t, tasksMap[t]), options);
            }
        }

        // Handles faulted worker task state - adds UI notification
        private void OnTaskFaulted(Task task, string prefix)
        {
            if (task.Exception != null)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    _mainWindow.AddUINotification(string.Format("ERROR: {0}. {1}", prefix, exception.Message));
                }
            }
        }
    }
}