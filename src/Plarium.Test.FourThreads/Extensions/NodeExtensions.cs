using System.Collections.Generic;
using System.IO;
using System.Linq;
using Plarium.Test.FourThreads.Model;

namespace Plarium.Test.FourThreads.Extensions
{
    internal static class FileSystemNodeExtensions
    {
        // Recursively selects all "leaves" - nodes with FileInfo, files
        public static IEnumerable<FileSystemNode> GetLeaves(this FileSystemNode node)
        {
            if (node == null)
            {
                return new List<FileSystemNode>();
            }

            if (node.Children.Count == 0 && node.Item is FileInfo)
            {
                return new List<FileSystemNode>() {node};
            }

            return node.Children.SelectMany(GetLeaves);
        }
    }
}