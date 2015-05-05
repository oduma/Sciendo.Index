using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sciendo.Music.Real.Lyrics.Provider
{
    public class WebDownloader : WebDownloaderBase
    {
        public override TOut TryQuery<TOut>(string url, Func<string> queryStringProvider=null,Func<string,TOut> transformToObject=null)
        {
            HttpClient httpClient = new HttpClient();
            var urlQueryString = "?";
            if (queryStringProvider != null)
                urlQueryString += queryStringProvider();
            using (var getTask = httpClient.GetStringAsync(new Uri(url + ((urlQueryString.TrimEnd()!="?")?urlQueryString:string.Empty)))
                .ContinueWith((p) => p).Result)
            {
                if (getTask.Status == TaskStatus.RanToCompletion || !string.IsNullOrEmpty(getTask.Result))
                {
                    if (transformToObject != null)
                        return transformToObject(getTask.Result);
                    if(typeof(TOut)!=typeof(string))
                        throw new Exception("No Transformation provided!");
                    return DefaultTransformation(getTask.Result) as TOut;
                }
                return null;
            }

        }
    }
}
