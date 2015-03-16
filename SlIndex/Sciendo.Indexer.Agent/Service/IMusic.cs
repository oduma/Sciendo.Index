using System.ServiceModel;

namespace Sciendo.Music.Agent.Service
{
    [ServiceContract(Namespace="http://Sciendo.Indexer.Agent")]
    public interface IMusic
    {
        [OperationContract]
        int IndexLyricsOnDemand(string fromPath);
        [OperationContract]
        int IndexMusicOnDemand(string fromPath);

        [OperationContract]
        int AcquireLyricsFor(string musicPath, bool retryFailed);

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
