using Sciendo.Common.Logging;
using Sciendo.Lyrics.Common;

namespace Sciendo.Music.Agent.Service.Solr.Mocks
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
