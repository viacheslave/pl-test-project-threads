using System.Collections.Generic;
using System.IO;

namespace Plarium.Test.FourThreads.Model
{
    // A composite tree of nodes for Advanced XML Worker
    internal class FileSystemTree
    {
        private List<FileSystemNode> Nodes { get; set; }

        public FileSystemTree()
        {
            Nodes = new List<FileSystemNode>();
        }

        public void AddTopLevelNode(FileSystemNode newNode)
        {
            Nodes.Add(newNode);
        }

        public FileSystemNode GetLastTopLevelNode()
        {
            return Nodes[Nodes.Count - 1];
        }

        public FileSystemNode GetPreLastTopLevelNode()
        {
            return Nodes[Nodes.Count - 2];
        }

        public bool HasSingleTopLevelNode()
        {
            return Nodes.Count == 1;
        }
    }

    // A tree node definition
    // Can represent both a folder and a file
    internal class FileSystemNode
    {
        public FileSystemInfo Item { get; set; }
        public List<FileSystemNode> Children { get; set; }
        public FileSystemNode Parent { get; set; }
        
        // Relevant only for folders
        public long Size { get; set; }

        public FileSystemNode()
        {
            Children = new List<FileSystemNode>();
        }

        public override string ToString()
        {
            return string.Format("[{0}] ({1} ch.) {2}", Item.Name, Children.Count, Size);
        }
    }
}