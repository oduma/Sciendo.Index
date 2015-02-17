using Newtonsoft.Json;
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
            Url = solrConnectionString;
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
            HttpClient httpClient = new HttpClient();
            using (var postTask = httpClient.PostAsJsonAsync<T>(new Uri(Url), package)
                .ContinueWith((p)=>p).Result)
            {
                if (postTask.Status != TaskStatus.RanToCompletion || !postTask.Result.IsSuccessStatusCode || postTask.Result.Content==null)
                    return new TrySendResponse { Status = Status.NotIndexed };

                using(var readTask = postTask.Result.Content.ReadAsStringAsync().ContinueWith((r)=>r).Result)
                {
                    if(readTask.Status!=TaskStatus.RanToCompletion || string.IsNullOrEmpty(readTask.Result))
                        return new TrySendResponse { Status = Status.NotIndexed};
                    var solrUpdateResponse = JsonConvert.DeserializeObject<SolrUpdateResponse>(readTask.Result);
                    if(solrUpdateResponse.responseHeader.status!=0)
                        return new TrySendResponse { Status = Status.NotIndexed, Time = solrUpdateResponse.responseHeader.QTime };
                    return new TrySendResponse { Status = Status.Done, Time = solrUpdateResponse.responseHeader.QTime };
                    
                }
            }

        }
    }
}
