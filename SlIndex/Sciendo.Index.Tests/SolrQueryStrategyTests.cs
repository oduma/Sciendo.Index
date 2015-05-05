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
            Assert.AreEqual("q=%22brown+girl%22&rows=20&start=23&wt=json&indent=true&stopwords=true&lowercaseOperators=true&defType=edismax&fl=lyrics+title+album+artist+file_path+file_path_id&qf=lyrics%5E10+title%5E5+album%5E3+artist%5E2+file_path%5E1&hl=true&hl.simple.pre=<em>&hl.simple.post=<%2Fem>&hl.requireFieldMatch=false&hl.highlightMultiTerm=true&hl.fl=lyrics+title+album+artist&facet=true&facet.mincount=1&facet.limit=-1&facet.missing=true&facet.field=artist_f&facet.field=extension_f&facet.field=letter_catalog_f", solrStrategy.GetQueryString());
        }

        [Test]
        public void SolrStrategy_GetFilter_ok()
        {
            SolrQueryStrategy solrStrategy = new SolrQueryStrategy("brown girl",20,23,"artist_f","The Doors");
            Assert.AreEqual("fq=artist_f%3A%22The+Doors%22&q=%22brown+girl%22&rows=20&start=23&wt=json&indent=true&stopwords=true&lowercaseOperators=true&defType=edismax&fl=lyrics+title+album+artist+file_path+file_path_id&qf=lyrics%5E10+title%5E5+album%5E3+artist%5E2+file_path%5E1&hl=true&hl.simple.pre=<em>&hl.simple.post=<%2Fem>&hl.requireFieldMatch=false&hl.highlightMultiTerm=true&hl.fl=lyrics+title+album+artist&facet=true&facet.mincount=1&facet.limit=-1&facet.missing=true&facet.field=artist_f&facet.field=extension_f&facet.field=letter_catalog_f", solrStrategy.GetFilterString());

        }

    }
}
