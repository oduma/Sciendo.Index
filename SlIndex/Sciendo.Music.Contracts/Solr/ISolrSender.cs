namespace Sciendo.Music.Contracts.Solr
{
    public interface ISolrSender
    {
        string Url { get; set; }
        TrySendResponse TrySend<T>(T package);
    }

}
