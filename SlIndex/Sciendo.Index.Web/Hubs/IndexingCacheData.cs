namespace Sciendo.Index.Web.Hubs
{
    public static class IndexingCacheData
    {

        public static bool ContinueMonitoring
        {
            get
            {
                return (new WebCacheProvider()).Get<bool>("continueMonitoring");
            }
            set
            {
                (new WebCacheProvider()).Put("continueMonitoring",value);
            }
        }
    }
}