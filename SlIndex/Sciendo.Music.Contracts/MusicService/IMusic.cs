using System.ServiceModel;

namespace Sciendo.Music.Contracts.MusicService
{
    [ServiceContract(Namespace="http://Sciendo.Music.Agent")]
    public interface IMusic
    {
        [OperationContract]
        int IndexLyricsOnDemand(string fromPath);
        [OperationContract]
        int IndexMusicOnDemand(string fromPath);

        [OperationContract]
        int AcquireLyricsOnDemandFor(string musicPath, bool retryFailed);

        int AcquireLyricsFor(string fromPath);

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
