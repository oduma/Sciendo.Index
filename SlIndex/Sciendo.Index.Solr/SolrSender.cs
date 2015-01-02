using System;
using System.Net.Http;

namespace Sciendo.Index.Solr
{
    public static class SolrSender
    {
        public static void Send(string url, Package package)
        {
            HttpClient httpClient = new HttpClient();
            using (var result = httpClient.PostAsJsonAsync(new Uri(url), package)
                .ContinueWith((p) => p).Result)
            {
                var resulTContent = result;
            }

        }
    }
}
