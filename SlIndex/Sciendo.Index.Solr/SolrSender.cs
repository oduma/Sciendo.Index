using Newtonsoft.Json;
using Sciendo.Common.Logging;
using Sciendo.Lyrics.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sciendo.Index.Solr
{
    public interface ISolrSender
    {
        string Url { get; set; }
        TrySendResponse TrySend<T>(T package);
    }

    public class SolrSender : ISolrSender
    {
        private string _url;
        public SolrSender(string solrConnectionString)
        {
            LoggingManager.Debug("Constructing SolrSender with: " +solrConnectionString);
            Url = solrConnectionString;
            LoggingManager.Debug("SolrSender constructed.");
        }

        public SolrSender()
        {
            
        }
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public virtual TrySendResponse TrySend<T>(T package)
        {
            LoggingManager.Debug("Send to solr...");
            HttpClient httpClient = new HttpClient();
            using (var postTask = httpClient.PostAsJsonAsync<T>(new Uri(Url), package)
                .ContinueWith((p)=>p).Result)
            {
                if (postTask.Status != TaskStatus.RanToCompletion || !postTask.Result.IsSuccessStatusCode ||
                    postTask.Result.Content == null)
                {
                    LoggingManager.Debug("Not send");
                    return new TrySendResponse { Status = Status.NotIndexed };
                }

                using(var readTask = postTask.Result.Content.ReadAsStringAsync().ContinueWith((r)=>r).Result)
                {
                    if (readTask.Status != TaskStatus.RanToCompletion || string.IsNullOrEmpty(readTask.Result))
                    {
                        LoggingManager.Debug("Not send.");
                        return new TrySendResponse { Status = Status.NotIndexed};
                    }
                    var solrUpdateResponse = JsonConvert.DeserializeObject<SolrUpdateResponse>(readTask.Result);
                    if (solrUpdateResponse.responseHeader.status != 0)
                    {
                        LoggingManager.Debug("Not send.");
                        return new TrySendResponse { Status = Status.NotIndexed, Time = solrUpdateResponse.responseHeader.QTime };
                    }
                    LoggingManager.Debug("Send");
                    return new TrySendResponse { Status = Status.Done, Time = solrUpdateResponse.responseHeader.QTime };
                    
                }
            }

        }
    }
}
