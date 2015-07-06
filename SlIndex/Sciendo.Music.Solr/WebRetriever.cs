using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sciendo.Music.Solr
{
    public static class WebRetriever
    {
        public static T TryRetrieve<T>(string url, string queryString,RequestType requestType) where T: class, new ()
        {
            return (requestType == RequestType.Get) ? TryGet<T>(url, queryString) : TryPost<T>(url, queryString);
        }
        private static T TryGet<T>(string url, string queryString) where T : class, new()
        {
            var httpClient = new HttpClient();
            using (var getTask = httpClient.GetStringAsync(new Uri(url+"?" +queryString))
                .ContinueWith(p => p).Result)
            {
                if (getTask.Status == TaskStatus.RanToCompletion || !string.IsNullOrEmpty(getTask.Result))
                {
                    return JsonConvert.DeserializeObject<T>(getTask.Result);
                }
                return null as T;
            }

        }


        private static T TryPost<T>(string url,string queryString) where T : class
        {
            var httpClient = new HttpClient();
            var query = new KeyValuePair<string, string>("q", queryString);

            //This is a hack
            if (!url.Contains("?wt=json"))
                url += "?wt=json";
           
            using (var postTask = httpClient.PostAsync(url, new FormUrlEncodedContent(new [] {query}))
                .ContinueWith(p => p).Result)
            {
                if (postTask.Status != TaskStatus.RanToCompletion || !postTask.Result.IsSuccessStatusCode ||
                    postTask.Result.Content == null)
                {
                    return null as T;
                }

                using (var readTask = postTask.Result.Content.ReadAsStringAsync().ContinueWith(r => r).Result)
                {
                    if (readTask.Status != TaskStatus.RanToCompletion || string.IsNullOrEmpty(readTask.Result))
                    {
                        return null as T;
                    }
                    return JsonConvert.DeserializeObject<T>(readTask.Result);
                }
            }

        }

    }
}
