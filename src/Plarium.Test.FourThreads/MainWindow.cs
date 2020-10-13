using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Plarium.Test.FourThreads.Extensions;
using Plarium.Test.FourThreads.Workers;

namespace Plarium.Test.FourThreads
{
    public partial class MainWindow : Form
    {
        // In-progress indicator
        private bool _inProgress;
        private readonly WorkerManager _workerManager;

        // Indicates that the application is being closed
        internal bool IsClosing;
        
        internal TextBox ControlTxtScanFolder
        {
            get
            {
                return txtScanFolder;
            }
        }

        internal TextBox ControlTxtXmlOutputFilePath
        {
            get
            {
                return txtXmlOutputFilePath;
            }
        }

        internal CheckBox ControlChkUseAdvancedXmlWorker
        {
            get
            {
                return chkUseAdvancedXmlWorker;
            }
        }

        internal CheckBox ControlChkEnableLiveStatistics
        {
            get
            {
                return chkEnableLiveStatistics;
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();

            txtScanFolder.Text = ScannerWorker.DefaultScanFolder;
            txtXmlOutputFilePath.Text = BaseQueueXmlWorker.DefaultXmlOutputFilePath;

            _workerManager = new WorkerManager(this);
        }

        // Handles Start/Stop user clicks
        private void btnStartScanning_Click(object sender, EventArgs e)
        {
            // If in-progress
            // Updates UI and sends cancel signal to the workers

            // If idle
            // Updates UI and start new workers

            if (_inProgress)
            {
                EnableControls(false, btnStartScanning);

                _workerManager.StopWorkers();
            }
            else
            {
                EnableControls(false, btnStartScanning, btnSelectScanFolder, btnSelectXmlOutputPathFile, chkEnableLiveStatistics, chkUseAdvancedXmlWorker);
                
                btnStartScanning.Text = @"Stop";
                tvFolder.Nodes.Clear();

                if (!chkEnableLiveStatistics.Checked)
                {
                    lblTreeWorkerQueueSize.Text = lblXmlWorkerQueueSize.Text = @"Not available";
                }
                
                _workerManager.StartNew();

                EnableControls(true, btnStartScanning);
                _inProgress = true;
            }
        }

        // Opens up a folder selection dialog to scan
        private void btnSelectScanFolder_Click(object sender, EventArgs e)
        {
            ShowSelectScanFolderDialog();
        }

        // Opens up a file save dialog - XML output file path
        private void btnSelectXmlOutputPathFile_Click(object sender, EventArgs e)
        {
            ShowSelectXmlOutputFileDialog();
        }
        
        // Opens a new Explorer window where XML output file is located
        private void btnOpenXmlOutputFolder_Click(object sender, EventArgs e)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("explorer.exe", Path.GetDirectoryName(txtXmlOutputFilePath.Text));
            new Process() { StartInfo = processStartInfo }.Start();
        }

        // If in-progress - sends cancel signal to the worker threads, waits and exits the application
        // If not - simply exits the application
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_inProgress && !IsClosing)
            {
                e.Cancel = true;
                IsClosing = true;

                AddUINotification("CANCELLING ALL WORKERS. Timeout 10 secs.");
                _workerManager.StopWorkers();
            }
        }

        // Handles updates from XML worker thread - current queue size
        internal void XmlWorkerOnStatisticsUpdated(object sender, QueueStatisticsEventArgs queueStatisticsEventArgs)
        {
            lblXmlWorkerQueueSize.InvokeIfRequired(this, () =>
            {
                lblXmlWorkerQueueSize.Text = queueStatisticsEventArgs.QueueSize.ToString();
            });
        }

        // Handles updates from Tree worker thread - current queue size
        internal void TreeWorkerOnStatisticsUpdated(object sender, QueueStatisticsEventArgs queueStatisticsEventArgs)
        {
            lblTreeWorkerQueueSize.InvokeIfRequired(this, () =>
            {
                lblTreeWorkerQueueSize.Text = queueStatisticsEventArgs.QueueSize.ToString();
            });
        }

        internal void WorkerOnNotification(object sender, NotificationEventArgs notificationEventArgs)
        {
            AddUINotification(notificationEventArgs.Item);
        }

        // Appends notifications to UI
        internal void AddUINotification(string info)
        {
            tbxMessages.InvokeIfRequired(this, () =>
            {
                tbxMessages.AppendText(info + Environment.NewLine);
                tbxMessages.ScrollToCaret();
            });
        }

        // Invoked after all worker tasks are completed/canceled, updates UI controls
        internal void PostProcessUI()
        {
            this.InvokeIfRequired(this, () =>
            {
                EnableControls(true, btnStartScanning, btnSelectScanFolder, btnSelectXmlOutputPathFile, chkEnableLiveStatistics, chkUseAdvancedXmlWorker);
                btnStartScanning.Text = @"Start";
                
                _inProgress = false;
            });
        }

        // Handles inserting of a new node to the TreeView control
        internal void UpdateTreeView(object sender, TreeViewUpdateEventArgs treeViewUpdateEventArgs)
        {
            if (treeViewUpdateEventArgs == null)
            {
                return;
            }

            TreeNode parentTreeNode = treeViewUpdateEventArgs.ParentTreeNode;
            TreeNode treeNode = treeViewUpdateEventArgs.TreeNode;

            tvFolder.InvokeIfRequired(this, () =>
            {
                if (parentTreeNode == null)
                {
                    tvFolder.Nodes.Add(treeNode);
                }
                else
                {
                    parentTreeNode.Nodes.Add(treeNode);
                }   
            });
        }

        private void ShowSelectScanFolderDialog()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = txtScanFolder.Text;
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.Description = @"Select a folder to scan";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtScanFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }
        
        private void ShowSelectXmlOutputFileDialog()
        {
            FileInfo fileInfo = new FileInfo(txtXmlOutputFilePath.Text);
            
            FileDialog fileDialog = new SaveFileDialog();
            fileDialog.CheckPathExists = true;
            fileDialog.FileName = fileInfo.Name;
            fileDialog.Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*";
            fileDialog.InitialDirectory = fileInfo.DirectoryName;
            fileDialog.RestoreDirectory = true;
            fileDialog.Title = @"Select a XML output file path";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtXmlOutputFilePath.Text = fileDialog.FileName;
            }
        }

        private void EnableControls(bool enable, params Control[] controls)
        {
            if (controls == null)
            {
                return;
            }

            foreach (Control control in controls)
            {
                control.InvokeIfRequired(this, 
                    () => 
                    {
                        control.Enabled = enable;
                    });
            }
        }
    }
}
