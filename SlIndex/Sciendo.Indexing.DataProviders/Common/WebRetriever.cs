using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sciendo.Music.DataProviders.Common
{
    public static class WebRetriever
    {
        public static T TryGet<T>(string url, Func<string> queryStringProvider) where T : class, new()
        {
            var httpClient = new HttpClient();
            using (var getTask = httpClient.GetStringAsync(new Uri(url+"?" +queryStringProvider()))
                .ContinueWith(p => p).Result)
            {
                if (getTask.Status == TaskStatus.RanToCompletion || !string.IsNullOrEmpty(getTask.Result))
                {
                    return JsonConvert.DeserializeObject<T>(getTask.Result);
                }
                return null as T;
            }

        }


        public static T TryPost<T>(string url,Func<string> queryStringProvider) where T : class
        {
            var httpClient = new HttpClient();
            var query = new KeyValuePair<string, string>("q", queryStringProvider());

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
