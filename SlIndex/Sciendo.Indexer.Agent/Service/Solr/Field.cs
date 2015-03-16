﻿using Newtonsoft.Json;

namespace Sciendo.Music.Agent.Service.Solr
{
    public class Field<T>
    {
        public Field()
        {
            Boost = 1d;
        }

        [JsonProperty("set")]
        public T Set { get; set; }

        [JsonProperty("boost")]
        public double Boost { get; set; }
    }
}
