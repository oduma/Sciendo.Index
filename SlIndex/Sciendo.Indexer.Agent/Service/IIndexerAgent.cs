using System.ServiceModel;

namespace Sciendo.Indexer.Agent.Service
{
    [ServiceContract(Namespace="http://Sciendo.Indexer.Agent")]
    public interface IIndexerAgent
    {
        [OperationContract]
        int IndexLyricsOnDemand(string fromPath);
        [OperationContract]
        int IndexMusicOnDemand(string fromPath);

        [OperationContract]
        ProgressStatus[] GetLastProcessedPackages();

        [OperationContract]
        string[] ListAvailableMusicPathsForIndexing(string fromPath);

        [OperationContract]
        string[] ListAvailableLyricsPathsForIndexing(string fromPath);

        [OperationContract]
        SourceFolders GetSourceFolders();
    }
}
