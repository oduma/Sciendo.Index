using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Solr;

namespace Sciendo.Music.Real.Solr
{
    public class SolrSender : ISolrSender
    {
        public SolrSender(string solrConnectionString)
        {
            LoggingManager.Debug("Constructing SolrSender with: " +solrConnectionString);
            Url = solrConnectionString;
            LoggingManager.Debug("SolrSender constructed.");
        }

        public SolrSender()
        {
            
        }

        public string Url { get; set; }

        public virtual TrySendResponse TrySend<T>(T package)
        {
            LoggingManager.Debug("Send to solr...");
            var httpClient = new HttpClient();
            using (var postTask = httpClient.PostAsJsonAsync(Url, package)
                .ContinueWith(p=>p).Result)
            {
                if (postTask.Status != TaskStatus.RanToCompletion || !postTask.Result.IsSuccessStatusCode ||
                    postTask.Result.Content == null)
                {
                    LoggingManager.Debug("Not send");
                    return new TrySendResponse { Status = Status.Error };
                }

                using(var readTask = postTask.Result.Content.ReadAsStringAsync().ContinueWith(r=>r).Result)
                {
                    if (readTask.Status != TaskStatus.RanToCompletion || string.IsNullOrEmpty(readTask.Result))
                    {
                        LoggingManager.Debug("Not send.");
                        return new TrySendResponse { Status = Status.Error};
                    }
                    var solrUpdateResponse = JsonConvert.DeserializeObject<SolrUpdateResponse>(readTask.Result);
                    if (solrUpdateResponse.responseHeader.Status != 0)
                    {
                        LoggingManager.Debug("Not send.");
                        return new TrySendResponse { Status = Status.Error, Time = solrUpdateResponse.responseHeader.Time };
                    }
                    LoggingManager.Debug("Send");
                    return new TrySendResponse { Status = Status.Done, Time = solrUpdateResponse.responseHeader.Time };
                    
                }
            }

        }
    }
}
