namespace Sciendo.Music.Solr.Strategies
{
    public interface ISolrQueryStrategy
    {
        string GetQueryString();
        string GetFilterString();
    }
}