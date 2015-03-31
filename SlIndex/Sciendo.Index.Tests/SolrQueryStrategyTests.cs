using NUnit.Framework;
using Sciendo.Music.DataProviders;

namespace Sciendo.Music.Tests
{
    [TestFixture]
    public class SolrQueryStrategyTests
    {
        [Test]
        public void SolrStrategy_GetQuery_ok()
        {
            SolrQueryStrategy solrStrategy = new SolrQueryStrategy("brown girl",20,23);
            Assert.AreEqual("q=brown girl&rows=20&start=23&wt=json&indent=true&stopwords=true&lowercaseOperators=true&defType=edismax&fl=lyrics+title+album+artist+file_path+file_path_id&qf=lyrics^10+title^5+album^3+artist^2+file_path^1&hl=true&hl.simple.pre=<em>&hl.simple.post=<%2Fem>&hl.requireFieldMatch=false&hl.highlightMultiTerm=true&hl.fl=lyrics+title+album+artist&facet=true&facet.mincount=1&facet.limit=-1&facet.missing=true&facet.field=artist_f&facet.field=extension_f&facet.field=letter_catalog_f", solrStrategy.GetQueryString());
        }

        [Test]
        public void SolrStrategy_GetFilter_ok()
        {
            SolrQueryStrategy solrStrategy = new SolrQueryStrategy("brown girl",20,23,"artist_f","The Doors");
            Assert.AreEqual("fq=artist_f:\"The Doors\"&q=brown girl&rows=20&start=23&wt=json&indent=true&stopwords=true&lowercaseOperators=true&defType=edismax&fl=lyrics+title+album+artist+file_path+file_path_id&qf=lyrics^10+title^5+album^3+artist^2+file_path^1&hl=true&hl.simple.pre=<em>&hl.simple.post=<%2Fem>&hl.requireFieldMatch=false&hl.highlightMultiTerm=true&hl.fl=lyrics+title+album+artist&facet=true&facet.mincount=1&facet.limit=-1&facet.missing=true&facet.field=artist_f&facet.field=extension_f&facet.field=letter_catalog_f", solrStrategy.GetFilterString());

        }

    }
}
