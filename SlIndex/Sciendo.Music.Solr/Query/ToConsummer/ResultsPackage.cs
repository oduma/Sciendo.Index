using Sciendo.Music.Solr.Query.Common;
namespace Sciendo.Music.Solr.Query.ToConsummer
{
    public class ResultsPackage
    {
        public Doc[] ResultRows { get; set; }
        public Field[] Fields { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
