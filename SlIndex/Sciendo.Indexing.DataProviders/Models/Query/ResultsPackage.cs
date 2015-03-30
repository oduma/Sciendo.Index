namespace Sciendo.Music.DataProviders.Models.Query
{
    public class ResultsPackage
    {
        public Doc[] ResultRows { get; set; }
        public Field[] Fields { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
