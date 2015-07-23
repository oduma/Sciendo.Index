﻿using System.ServiceModel;
using Sciendo.Music.Contracts.Monitoring;

namespace Sciendo.Music.Contracts.MusicService
{
    [ServiceContract(Namespace = "http://Sciendo.Music.Agent")]
    public interface IMusic
    {
        [OperationContract]
        int IndexOnDemand(string fromPath);

        [OperationContract(IsOneWay=true)]
        void Index(string fromPath,ProcessType processType);

        [OperationContract]
        int AcquireLyricsOnDemandFor(string musicPath, bool retryFailed);

        [OperationContract]
        string[] ListAvailablePathsForIndexing(string fromPath);

        [OperationContract]
        string GetSourceFolder();

        [OperationContract]
        int UnIndexOnDemand(string musicFile);

        [OperationContract]
        WorkingSet GetCurrentWorkingSet();
    }
}