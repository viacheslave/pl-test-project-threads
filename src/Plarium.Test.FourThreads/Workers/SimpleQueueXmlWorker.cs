using System.IO;

namespace Plarium.Test.FourThreads.Workers
{
    // Concrete worker
    // Creates XML output file with NO folder sizes
    // Sequential write, fast
    internal class SimpleQueueXmlWorker : BaseQueueXmlWorker
    {
        // Current tree level
        private DirectoryInfo _currentParentFolder;

        public SimpleQueueXmlWorker(XmlWorkerParameters workerParameters) : base(workerParameters)
        {
            _currentParentFolder = new DirectoryInfo(((XmlWorkerParameters)Parameters).ScanFolder);
        }
        
        // Processes new item
        protected override void ProcessItem(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo == null)
            {
                return;
            }

            // Looks for parent folder to preserve tree structure in XML
            DirectoryInfo itemParentFolder = GetParentFolder(fileSystemInfo);
            IterateThroughParentFolders(itemParentFolder);
            
            // Writes Folder tag
            if (fileSystemInfo is DirectoryInfo)
            {
                _currentParentFolder = fileSystemInfo as DirectoryInfo;

                _xmlWriter.WriteStartElement("Folder");
                FillNodeAttributes(fileSystemInfo);
            }
            
            // Writes File tag
            if (fileSystemInfo is FileInfo)
            {
                _xmlWriter.WriteStartElement("File");
                FillNodeAttributes(fileSystemInfo);
                _xmlWriter.WriteEndElement();
            }
        }
        
        // Recursively looks for parent folder
        private void IterateThroughParentFolders(DirectoryInfo parentFolder)
        {
            if (parentFolder.FullName == _currentParentFolder.FullName)
            {
                return;
            }

            _xmlWriter.WriteEndElement();

            _currentParentFolder = _currentParentFolder.Parent;
            IterateThroughParentFolders(parentFolder);
        }
    }
}