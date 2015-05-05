using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Solr;

namespace Sciendo.Music.Mocks.Solr
{
    public class MockSender:ISolrSender
    {
        public string Url { get; set; }

        public  TrySendResponse TrySend<T>(T package)
        {
            LoggingManager.Debug("Mock Try Send.");
            return new TrySendResponse {Status = Status.Done, Time=.0};
        }
    }
}
