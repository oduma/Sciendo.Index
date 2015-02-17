using System.ServiceModel;
namespace Sciendo.Indexer.Agent
{
    [ServiceContract(Name="IndexerAgent",Namespace="Sciendo.Indexer.Agent")]
    public interface IIndexerAgent
    {
        [OperationContract]
        int IndexLyricsOnDemand(string fromPath);
        [OperationContract]
        int IndexMusicOnDemand(string fromPath);

    }
}
