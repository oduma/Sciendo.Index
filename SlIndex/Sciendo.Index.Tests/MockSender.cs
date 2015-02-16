using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Index.Tests
{
    public class MockSender:SolrSender
    {
        public MockSender() : base(string.Empty)
        {
        }

        public override TrySendResponse TrySend<T>(T package)
        {
            return new TrySendResponse {Status = Status.Done, Time=.0};
        }
    }
}
