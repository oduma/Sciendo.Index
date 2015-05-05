using Sciendo.Music.Real;
using System;
using Sciendo.Music.Real.Lyrics.Provider;

namespace Sciendo.Music.Mocks.Processing
{
    public class MockDownloader:WebDownloaderBase
    {
        public override TOut TryQuery<TOut>(string url, Func<string> queryStringProvider = null, Func<string, TOut> transformToObject = null)
        {
            if (url == @"http://lyrics.wikia.com/api.php?func=getSong&artist=my_artist&song=my_song&fmt=xml")
                return DefaultTransformation("my download") as TOut;
            else if(url.Contains("not_found"))
            {
                return DefaultTransformation("no lyrics") as TOut;
            }
            else
            {
                return DefaultTransformation("post processing") as TOut;
            }
        }
    }
}
