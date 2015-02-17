﻿using System.ServiceModel;
namespace Sciendo.Indexer.Agent
{
    [ServiceContract(Namespace="http://Sciendo.Indexer.Agent")]
    public interface IIndexerAgent
    {
        [OperationContract]
        int IndexLyricsOnDemand(string fromPath);
        [OperationContract]
        int IndexMusicOnDemand(string fromPath);

    }
}
