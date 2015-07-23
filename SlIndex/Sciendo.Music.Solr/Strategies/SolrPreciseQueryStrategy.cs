using System;
using System.Net;

namespace Sciendo.Music.Solr.Strategies
{
    public class SolrPreciseQueryStrategy:ISolrQueryStrategy
    {
        private string _preciseQuery;

        public SolrPreciseQueryStrategy(string preciseQuery)
        {
            this._preciseQuery = preciseQuery;
        }

        public string GetQueryString()
        {
            return string.Format("wt=json&q={0}",_preciseQuery);
        }

        public string GetFilterString()
        {
            throw new NotImplementedException();
        }
    }
}
