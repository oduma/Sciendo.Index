namespace Sciendo.Music.DataProviders
{
    public interface ISolrQueryStrategy
    {
        string GetQueryString();
        string GetFilterString();
    }
}