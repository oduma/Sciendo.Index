using System.ServiceModel;
using Sciendo.Music.Contracts.Monitoring;

namespace Sciendo.Music.Contracts.MusicService
{
    [ServiceContract(Namespace="http://Sciendo.Music.Agent")]
    public interface IMusic
    {
        [OperationContract]
        int IndexLyricsOnDemand(string fromPath);
        [OperationContract]
        int IndexMusicOnDemand(string fromPath);

        int IndexLyrics(string fromPath, ProcessType processType);

        int IndexMusic(string fromPath, ProcessType processType);

        [OperationContract]
        int AcquireLyricsOnDemandFor(string musicPath, bool retryFailed);

        int AcquireLyricsFor(string fromPath, ProcessType processType);

        [OperationContract]
        ProgressStatus[] GetLastProcessedPackages();

        [OperationContract]
        string[] ListAvailableMusicPathsForIndexing(string fromPath);

        [OperationContract]
        string[] ListAvailableLyricsPathsForIndexing(string fromPath);

        [OperationContract]
        SourceFolders GetSourceFolders();

        [OperationContract]
        int UnIndexMusicOnDemand(string musicFile);

        [OperationContract]
        bool DeleteLyricsFile(string file);
    }
}
