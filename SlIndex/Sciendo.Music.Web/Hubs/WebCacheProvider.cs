using System.Web;

namespace Sciendo.Music.Web.Hubs
{
    public class WebCacheProvider : ICacheProvider
    {
        private object _lock= new object();

        public T Get<T>(string key)
        {
            lock (_lock)
            {
                var obj = HttpContext.Current.Cache.Get(key);
                if (obj == null)
                    return default(T);
                return (T)obj;
            }
        }

        public void Put<T>(string key, T value)
        {
            lock (_lock)
            {
                HttpContext.Current.Cache.Insert(key,value);
            }
        }
    }
}