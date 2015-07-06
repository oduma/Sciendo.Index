using Newtonsoft.Json;

namespace Sciendo.Music.Solr.Query.FromSolr
{
    public class Facets
    {
        [JsonProperty("facet_queries")]
        public object FacetQueries { get; set; }
        [JsonProperty("facet_fields")]
        public FacetFields FacetFields { get; set; }
        [JsonProperty("facet_dates")]
        public object FacetDates { get; set; }
        [JsonProperty("facet_intervals")]
        public object FacetIntervals { get; set; }
        [JsonProperty("facet_ranges")]
        public object FacetRanges { get; set; }
    }
}
