using System;

namespace Sciendo.Music.Agent.LyricsProvider.Mocks
{
    public class MockDownloader:WebDownloaderBase
    {
        public override TOut TryQuery<TOut>(string url, Func<string> queryStringProvider = null, Func<string, TOut> transformToObject = null)
        {
            throw new NotImplementedException();
        }
    }
}
