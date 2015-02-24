using System;

namespace Sciendo.Lyrics.Provider.Service
{
    public abstract class WebDownloaderBase
    {
        public abstract TOut TryQuery<TOut>(string url, Func<string> queryStringProvider=null,Func<string,TOut> transformToObject=null) where TOut: class;

        protected string DefaultTransformation(string inString)
        {
            return inString;
        }
    }
}