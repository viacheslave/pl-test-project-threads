using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Xml;
using Plarium.Test.FourThreads.Extensions;

namespace Plarium.Test.FourThreads.Workers
{
    // Base class for XML workers
    internal abstract class BaseQueueXmlWorker : BaseQueueWorker
    {
        protected XmlWriter _xmlWriter;

        protected BaseQueueXmlWorker(BaseWorkerParameters workerParameters) : base(workerParameters)
        {
        }

        // Creates and initializes new XML Writer
        protected void CreateXmlWriter()
        {
            string filePath = ((XmlWorkerParameters) Parameters).XmlFileLocation;

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Encoding = Encoding.Unicode,
                Indent = true,
                ConformanceLevel = ConformanceLevel.Document
            };

            _xmlWriter = XmlWriter.Create(filePath, xmlWriterSettings);

            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("Data");
        }

        // Adds a file/folder attributes
        // Name, Attributes, CreateDate, ModifiedDate, LastAccessDate, Size (for files), Owner and Effective permissions
        protected void FillNodeAttributes(FileSystemInfo fileSystemInfo)
        {
            WriteAttribute("Name", fileSystemInfo.Name);
            WriteAttribute("Attributes", fileSystemInfo.Attributes.ToString());
            WriteAttribute("CreatedOn", fileSystemInfo.CreationTime.FormatDateForXml());
            WriteAttribute("ModifiedOn", fileSystemInfo.LastWriteTime.FormatDateForXml());
            WriteAttribute("LastAccessedOn", fileSystemInfo.LastAccessTime.FormatDateForXml());

            if (fileSystemInfo is FileInfo)
            {
                FileInfo fileInfo = fileSystemInfo as FileInfo;
                WriteAttribute("Size", fileInfo.Length.ToString());
            } 
            
            WriteFileSystemObjectOwner(fileSystemInfo);
        }

        // Writes Owner and Effective Permissions
        private void WriteFileSystemObjectOwner(FileSystemInfo fileSystemInfo)
        {
            try
            {
                FileSystemSecurity accessControl = null;

                if (fileSystemInfo is FileInfo)
                {
                    FileInfo fileInfo = fileSystemInfo as FileInfo;
                    accessControl = File.GetAccessControl(fileInfo.FullName);
                }

                if (fileSystemInfo is DirectoryInfo)
                {
                    DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
                    accessControl = Directory.GetAccessControl(directoryInfo.FullName);
                }

                WriteAttribute("Owner", accessControl.GetOwner());
                WriteAttribute("Permissions", accessControl.GetEffectivePermissions());
            }
            catch (Exception exception)
            {
                NotifyExceptionOccured(exception);
            }
        }

        private void WriteAttribute(string attributeName, string attributeValue)
        {
            _xmlWriter.WriteStartAttribute(attributeName);
            _xmlWriter.WriteValue(attributeValue);
            _xmlWriter.WriteEndAttribute();
        }

        // Flushes and closes the XML Writer
        protected void DisposeXmlWriter()
        {
            if (_xmlWriter != null)
            {
                try
                {
                    _xmlWriter.WriteEndDocument();
                }
                catch (Exception)
                {
                }

                _xmlWriter.Flush();
                _xmlWriter.Close();
            }
        }

        protected override void Prepare()
        {
            base.Prepare();
            CreateXmlWriter();
        }

        protected override void Dispose()
        {
            base.Dispose();
            DisposeXmlWriter();
        }

        // Default XML output file path
        public static string DefaultXmlOutputFilePath
        {
            get 
            { 
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                    string.Format("scanOutput_{0}.xml", DateTime.Now.FormatDateForFileName())); 
            }
        }
    }
}