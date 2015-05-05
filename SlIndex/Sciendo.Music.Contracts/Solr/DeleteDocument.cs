using Newtonsoft.Json;

namespace Sciendo.Music.Contracts.Solr
{
    public class DeleteDocument
    {
        public DeleteDocument()
        {
            
        }
        public DeleteDocument(string file)
        {
            DeleteById = new DocumentId {Id = file.ToLower()};
        }

        [JsonProperty("delete")]
        public DocumentId DeleteById { get; set; }
    }
}
