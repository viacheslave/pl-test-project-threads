namespace Plarium.Test.FourThreads
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartScanning = new System.Windows.Forms.Button();
            this.tvFolder = new System.Windows.Forms.TreeView();
            this.tbxMessages = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblXmlWorkerQueueSize = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTreeWorkerQueueSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtScanFolder = new System.Windows.Forms.TextBox();
            this.btnSelectScanFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSelectXmlOutputPathFile = new System.Windows.Forms.Button();
            this.txtXmlOutputFilePath = new System.Windows.Forms.TextBox();
            this.chkUseAdvancedXmlWorker = new System.Windows.Forms.CheckBox();
            this.btnOpenXmlOutputFolder = new System.Windows.Forms.Button();
            this.chkEnableLiveStatistics = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartScanning
            // 
            this.btnStartScanning.Location = new System.Drawing.Point(735, 9);
            this.btnStartScanning.Name = "btnStartScanning";
            this.btnStartScanning.Size = new System.Drawing.Size(152, 76);
            this.btnStartScanning.TabIndex = 0;
            this.btnStartScanning.Text = "Start";
            this.btnStartScanning.UseVisualStyleBackColor = true;
            this.btnStartScanning.Click += new System.EventHandler(this.btnStartScanning_Click);
            // 
            // tvFolder
            // 
            this.tvFolder.Location = new System.Drawing.Point(12, 70);
            this.tvFolder.Name = "tvFolder";
            this.tvFolder.Size = new System.Drawing.Size(428, 366);
            this.tvFolder.TabIndex = 2;
            // 
            // tbxMessages
            // 
            this.tbxMessages.Enabled = false;
            this.tbxMessages.Location = new System.Drawing.Point(447, 183);
            this.tbxMessages.Name = "tbxMessages";
            this.tbxMessages.ReadOnly = true;
            this.tbxMessages.Size = new System.Drawing.Size(439, 253);
            this.tbxMessages.TabIndex = 4;
            this.tbxMessages.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblXmlWorkerQueueSize);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblTreeWorkerQueueSize);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(447, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 61);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Statistics";
            // 
            // lblXmlWorkerQueueSize
            // 
            this.lblXmlWorkerQueueSize.AutoSize = true;
            this.lblXmlWorkerQueueSize.Location = new System.Drawing.Point(141, 38);
            this.lblXmlWorkerQueueSize.Name = "lblXmlWorkerQueueSize";
            this.lblXmlWorkerQueueSize.Size = new System.Drawing.Size(13, 13);
            this.lblXmlWorkerQueueSize.TabIndex = 5;
            this.lblXmlWorkerQueueSize.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "XML Worker Queue Size:";
            // 
            // lblTreeWorkerQueueSize
            // 
            this.lblTreeWorkerQueueSize.AutoSize = true;
            this.lblTreeWorkerQueueSize.Location = new System.Drawing.Point(141, 23);
            this.lblTreeWorkerQueueSize.Name = "lblTreeWorkerQueueSize";
            this.lblTreeWorkerQueueSize.Size = new System.Drawing.Size(13, 13);
            this.lblTreeWorkerQueueSize.TabIndex = 2;
            this.lblTreeWorkerQueueSize.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Tree Worker Queue Size:";
            // 
            // txtScanFolder
            // 
            this.txtScanFolder.Enabled = false;
            this.txtScanFolder.Location = new System.Drawing.Point(87, 14);
            this.txtScanFolder.Name = "txtScanFolder";
            this.txtScanFolder.ReadOnly = true;
            this.txtScanFolder.Size = new System.Drawing.Size(308, 20);
            this.txtScanFolder.TabIndex = 6;
            // 
            // btnSelectScanFolder
            // 
            this.btnSelectScanFolder.Location = new System.Drawing.Point(400, 12);
            this.btnSelectScanFolder.Name = "btnSelectScanFolder";
            this.btnSelectScanFolder.Size = new System.Drawing.Size(38, 23);
            this.btnSelectScanFolder.TabIndex = 7;
            this.btnSelectScanFolder.Text = "...";
            this.btnSelectScanFolder.UseVisualStyleBackColor = true;
            this.btnSelectScanFolder.Click += new System.EventHandler(this.btnSelectScanFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Scan Folder:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "XML Output:";
            // 
            // btnSelectXmlOutputPathFile
            // 
            this.btnSelectXmlOutputPathFile.Location = new System.Drawing.Point(400, 38);
            this.btnSelectXmlOutputPathFile.Name = "btnSelectXmlOutputPathFile";
            this.btnSelectXmlOutputPathFile.Size = new System.Drawing.Size(38, 23);
            this.btnSelectXmlOutputPathFile.TabIndex = 10;
            this.btnSelectXmlOutputPathFile.Text = "...";
            this.btnSelectXmlOutputPathFile.UseVisualStyleBackColor = true;
            this.btnSelectXmlOutputPathFile.Click += new System.EventHandler(this.btnSelectXmlOutputPathFile_Click);
            // 
            // txtXmlOutputFilePath
            // 
            this.txtXmlOutputFilePath.Enabled = false;
            this.txtXmlOutputFilePath.Location = new System.Drawing.Point(87, 40);
            this.txtXmlOutputFilePath.Name = "txtXmlOutputFilePath";
            this.txtXmlOutputFilePath.ReadOnly = true;
            this.txtXmlOutputFilePath.Size = new System.Drawing.Size(308, 20);
            this.txtXmlOutputFilePath.TabIndex = 9;
            // 
            // chkUseAdvancedXmlWorker
            // 
            this.chkUseAdvancedXmlWorker.AutoSize = true;
            this.chkUseAdvancedXmlWorker.Location = new System.Drawing.Point(457, 14);
            this.chkUseAdvancedXmlWorker.Name = "chkUseAdvancedXmlWorker";
            this.chkUseAdvancedXmlWorker.Size = new System.Drawing.Size(156, 17);
            this.chkUseAdvancedXmlWorker.TabIndex = 12;
            this.chkUseAdvancedXmlWorker.Text = "Use advanced XML worker";
            this.chkUseAdvancedXmlWorker.UseVisualStyleBackColor = true;
            // 
            // btnOpenXmlOutputFolder
            // 
            this.btnOpenXmlOutputFolder.Location = new System.Drawing.Point(735, 91);
            this.btnOpenXmlOutputFolder.Name = "btnOpenXmlOutputFolder";
            this.btnOpenXmlOutputFolder.Size = new System.Drawing.Size(152, 23);
            this.btnOpenXmlOutputFolder.TabIndex = 13;
            this.btnOpenXmlOutputFolder.Text = "Open XML output folder";
            this.btnOpenXmlOutputFolder.UseVisualStyleBackColor = true;
            this.btnOpenXmlOutputFolder.Click += new System.EventHandler(this.btnOpenXmlOutputFolder_Click);
            // 
            // chkEnableLiveStatistics
            // 
            this.chkEnableLiveStatistics.AutoSize = true;
            this.chkEnableLiveStatistics.Location = new System.Drawing.Point(457, 34);
            this.chkEnableLiveStatistics.Name = "chkEnableLiveStatistics";
            this.chkEnableLiveStatistics.Size = new System.Drawing.Size(127, 17);
            this.chkEnableLiveStatistics.TabIndex = 14;
            this.chkEnableLiveStatistics.Text = "Enable Live Statistics";
            this.chkEnableLiveStatistics.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 443);
            this.Controls.Add(this.chkEnableLiveStatistics);
            this.Controls.Add(this.btnOpenXmlOutputFolder);
            this.Controls.Add(this.chkUseAdvancedXmlWorker);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSelectXmlOutputPathFile);
            this.Controls.Add(this.txtXmlOutputFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectScanFolder);
            this.Controls.Add(this.txtScanFolder);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbxMessages);
            this.Controls.Add(this.tvFolder);
            this.Controls.Add(this.btnStartScanning);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Plarium Test Four Threads";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartScanning;
        private System.Windows.Forms.TreeView tvFolder;
        private System.Windows.Forms.RichTextBox tbxMessages;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTreeWorkerQueueSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblXmlWorkerQueueSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtScanFolder;
        private System.Windows.Forms.Button btnSelectScanFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSelectXmlOutputPathFile;
        private System.Windows.Forms.TextBox txtXmlOutputFilePath;
        private System.Windows.Forms.CheckBox chkUseAdvancedXmlWorker;
        private System.Windows.Forms.Button btnOpenXmlOutputFolder;
        private System.Windows.Forms.CheckBox chkEnableLiveStatistics;
    }
}

