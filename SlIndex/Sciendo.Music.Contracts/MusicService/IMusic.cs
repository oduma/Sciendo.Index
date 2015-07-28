using System.ServiceModel;
using Sciendo.Music.Contracts.Monitoring;

namespace Sciendo.Music.Contracts.MusicService
{
    [ServiceContract(Namespace = "http://Sciendo.Music.Agent")]
    public interface IMusic
    {
        [OperationContract(IsOneWay=true)]
        void IndexOnDemand(string fromPath);

        [OperationContract(IsOneWay=true)]
        void Index(string fromPath,ProcessType processType);

        [OperationContract(IsOneWay=true)]
        void AcquireLyricsOnDemandFor(string musicPath, bool retryFailed);

        [OperationContract]
        string[] ListAvailablePathsForIndexing(string fromPath);

        [OperationContract]
        string GetSourceFolder();

        [OperationContract(IsOneWay=true)]
        void UnIndexOnDemand(string musicFile);

        [OperationContract]
        WorkingSet GetCurrentWorkingSet();
    }
}