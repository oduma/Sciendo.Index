using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sciendo.Music.DataProviders.SolrContracts;

namespace Sciendo.Music.DataProviders.Mocks
{
    public class DictionariesConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType.GenericTypeArguments[1].Name == "Highlighting")
                return ReadHighlighting(reader);
            if(objectType.GenericTypeArguments[1].Name == "Int32")
                return ReadFacetFields(reader);
            return null;
        }

        private static object ReadFacetFields(JsonReader reader)
        {
            try
            {
                var item = JObject.Load(reader);

                var facetField = new Dictionary<string, int>();
                foreach (var itemProperty in item.Properties())
                {
                    string key = (itemProperty.Name) ?? "Unknown";
                    facetField.Add(key, (int)itemProperty.Value);
                }

                return facetField;

            }
            catch
            {
                return null;
            }
        }

        private static object ReadHighlighting(JsonReader reader)
        {
            var item = JObject.Load(reader);
            var highlightings = new Dictionary<string, Highlighting>();
            foreach (var itemProperty in item.Properties())
            {
                var highlighting = JsonConvert.DeserializeObject<Highlighting>(itemProperty.Value.ToString());
                highlightings.Add(itemProperty.Name, highlighting);
            }
            return highlightings;
        }
        public override bool CanConvert(Type objectType)
        {
            if(objectType.Name.Contains("Dictionary"))
            {
                return true;
            }
            return false;
        }
    }
}
