using System.Threading;

namespace Plarium.Test.FourThreads.Workers
{
    // Various worker type parameters
    // Cancellation is supported is every worker
    
    internal class BaseWorkerParameters
    {
        public CancellationToken CancellationToken { get; set; }
    }

    internal class ScannerWorkerParameters : BaseWorkerParameters
    {
        public string ScanFolder { get; set; }
    }

    internal class XmlWorkerParameters : ScannerWorkerParameters
    {
        public string XmlFileLocation { get; set; }
    }
}