using System;

namespace Sciendo.Music.DataProviders
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
            return _preciseQuery;
        }

        public string GetFilterString()
        {
            throw new NotImplementedException();
        }
    }
}
