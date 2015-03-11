﻿namespace Sciendo.Index.Web.Hubs
{
    public interface ICacheProvider
    {
        T Get<T>(string key);
        void Put<T>(string key, T value);
    }
}