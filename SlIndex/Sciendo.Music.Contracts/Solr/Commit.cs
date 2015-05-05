using Newtonsoft.Json;

namespace Sciendo.Music.Contracts.Solr
{
    public class Commit
    {
        [JsonProperty("commit")]
        public object CommitFlag { get; set; }

        public Commit()
        {
            CommitFlag = new object();
        }

    }
}
