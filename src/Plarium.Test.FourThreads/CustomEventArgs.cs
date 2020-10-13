using System;
using System.IO;
using System.Windows.Forms;

namespace Plarium.Test.FourThreads
{
    // Various custom type arguments for event handlers between workers
    
    internal class FileSystemInfoEventArgs : EventArgs
    {
        public FileSystemInfo Item { get; set; }
    }

    internal class NotificationEventArgs : EventArgs
    {
        public string Item { get; set; }
    }

    internal class QueueStatisticsEventArgs : EventArgs
    {
        public long QueueSize { get; set; }
    }

    internal class TreeViewUpdateEventArgs : EventArgs
    {
        public TreeNode ParentTreeNode { get; set; }
        public TreeNode TreeNode { get; set; }
    }
}