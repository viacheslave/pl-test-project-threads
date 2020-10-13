using System.IO;
using Plarium.Test.FourThreads.Model;

namespace Plarium.Test.FourThreads.Workers
{
    // Concrete worker
    // Creates XML output file WITH folder sizes
    // Writes by blocks of data, slow
    internal class AdvancedQueueXmlWorker : BaseQueueXmlWorker
    {
        // Inner tree structure
        private readonly FileSystemTree _tree;

        // Current tree level
        private FileSystemNode _currentParentNode;
        
        public AdvancedQueueXmlWorker(XmlWorkerParameters workerParameters) : base(workerParameters)
        {
            _tree = new FileSystemTree();
        }

        // Processes new item
        // Creates new tree node, adds it to the tree
        protected override void ProcessItem(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo == null)
            {
                return;
            }

            DirectoryInfo parentFolder = GetParentFolder(fileSystemInfo);

            FileSystemNode newNode = new FileSystemNode
            {
                Item = fileSystemInfo
            };

            var parentNode = FindParentNode(parentFolder);
            if (parentNode == null)
            {
                _tree.AddTopLevelNode(newNode);
                
                // If new top level node is inserted - previous node (with all children) can be saved to XML (and, probably removed from tree)
                SaveNode();
            }
            else
            {
                newNode.Parent = parentNode;
                parentNode.Children.Add(newNode);
            }

            if (fileSystemInfo is DirectoryInfo)
            {
                _currentParentNode = newNode;
            }
            else
            {
                _currentParentNode = parentNode;

                // If file node is inserted - recalculate sizes of all parents up to the top
                RecursiveryRecalculateFoldersSizes(newNode);
            }
        }

        // Updates folder sizes recursively
        private void RecursiveryRecalculateFoldersSizes(FileSystemNode node)
        {
            if (node != null && node.Item is FileInfo)
            {
                long size = (node.Item as FileInfo).Length;
                RecalculateParentFolderSize(node, size);
            }
        }

        private void RecalculateParentFolderSize(FileSystemNode node, long extraSize)
        {
            if (node == null)
            {
                return;
            }
            
            if (node.Parent != null)
            {
                node.Parent.Size += extraSize;
                RecalculateParentFolderSize(node.Parent, extraSize);
            }
        }

        // Recursively looks for parent tree node in order to insert a new node
        private FileSystemNode FindParentNode(DirectoryInfo itemParentFolder)
        {
            if (_currentParentNode == null)
            {
                return null;
            }
            
            if (_currentParentNode.Item.FullName == itemParentFolder.FullName)
            {
                return _currentParentNode;
            }

            _currentParentNode = _currentParentNode.Parent;
            return FindParentNode(itemParentFolder);
        }

        private void SaveNode(FileSystemNode node = null)
        {
            if (_tree.HasSingleTopLevelNode())
            {
                return;
            }

            if (node == null)
            {
                node = _tree.GetPreLastTopLevelNode();
            }

            FlushNode(node);
        }

        // Saves last top-level node to XML
        protected override void Dispose()
        {
            SaveNode(_tree.GetLastTopLevelNode());
            base.Dispose();
        }

        // Recursively saves the node to XML
        private void FlushNode(FileSystemNode node)
        {
            if (node == null)
            {
                return;
            }

            string elementName = node.Item is DirectoryInfo
                ? "Folder"
                : "File";

            _xmlWriter.WriteStartElement(elementName);

            FillNodeAttributes(node.Item);

            if (node.Item is DirectoryInfo)
            {
                _xmlWriter.WriteAttributeString("Size", node.Size.ToString());

                foreach (FileSystemNode child in node.Children)
                {
                    FlushNode(child);
                }
            }
   
            _xmlWriter.WriteEndElement();
        }
    }
}