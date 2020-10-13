using System;
using System.IO;
using System.Windows.Forms;

namespace Plarium.Test.FourThreads.Workers
{
    // Concrete worker
    // Builds the TreeView structure
    internal class TreeQueueWorker : BaseQueueWorker
    {
        // Raised when the UI TreeView control should be updated
        public event EventHandler<TreeViewUpdateEventArgs> TreeViewUpdate;

        // Current tree level
        private TreeNode _currentFolderNode;
        
        public TreeQueueWorker(BaseWorkerParameters workerParameters) : base(workerParameters)
        {
        }

        // Processes new item
        protected override void ProcessItem(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo == null)
            {
                return;
            }

            DirectoryInfo parentFolder = GetParentFolder(fileSystemInfo);

            // Calculates and creates current Node and its parent Node
            TreeNode treeNode = BuildNewTreeNode(fileSystemInfo);
            TreeNode parentTreeNode = FindParentNode(_currentFolderNode, parentFolder);
            
            // Updates current tree level
            UpdateCurrentTreeLevelInfo(fileSystemInfo, treeNode, parentTreeNode);

            // Insert a new Node to UI TreeView
            InvokeTreeViewUpdate(parentTreeNode, treeNode);
        }

        // Creates new TreeNode element
        private static TreeNode BuildNewTreeNode(FileSystemInfo fileSystemInfo)
        {
            string nodeName = fileSystemInfo is DirectoryInfo
                ? string.Format("[{0}]", fileSystemInfo.Name)
                : fileSystemInfo.Name;

            TreeNode treeNode = new TreeNode(nodeName)
            {
                Tag = fileSystemInfo.FullName
            };

            return treeNode;
        }

        // Next two methods recursively verify current tree level - current folder pointer
        // When a new Node is created the algorithm seeks for a valid parent Node to insert the child into
        
        private void UpdateCurrentTreeLevelInfo(FileSystemInfo fileSystemInfo, TreeNode treeNode, TreeNode parentTreeNode)
        {
            if (fileSystemInfo is DirectoryInfo)
            {
                _currentFolderNode = treeNode;
            }

            if (fileSystemInfo is FileInfo && parentTreeNode == null)
            {
                _currentFolderNode = null;
            }
        }

        private TreeNode FindParentNode(TreeNode parentTreeNode, DirectoryInfo directoryInfo)
        {
            if (parentTreeNode == null)
            {
                return null;
            }

            if (parentTreeNode.Tag != null)
            {
                if (directoryInfo.FullName == (string)parentTreeNode.Tag)
                {
                    return parentTreeNode;
                }

                return FindParentNode(parentTreeNode.Parent, directoryInfo);
            }

            return null;
        }

        private void InvokeTreeViewUpdate(TreeNode parentTreeNode, TreeNode treeNode)
        {
            TreeViewUpdateEventArgs treeViewUpdateEventArgs = new TreeViewUpdateEventArgs()
            {
                ParentTreeNode = parentTreeNode,
                TreeNode = treeNode
            };

            OnTreeViewUpdate(treeViewUpdateEventArgs);
        }

        private void OnTreeViewUpdate(TreeViewUpdateEventArgs treeViewUpdateEventArgs)
        {
            if (TreeViewUpdate != null)
            {
                TreeViewUpdate(this, treeViewUpdateEventArgs);
            }
        }
    }
}