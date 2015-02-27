using System;
using System.IO;
using Newtonsoft.Json;

namespace Sciendo.Indexer.Agent.Service.Solr
{
    public class Document
    {
        private const string Prefix = "file:///";
        public Document()
        { }

        public Document (string filePath, string catalogLetter)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");
            FilePathId = filePath.ToLower();
            file_path = new Field<string> { Set = Prefix + filePath };

            ExtensionF = new Field<string> { Set = Path.GetExtension(FilePathId).Replace(".", "") };
            LetterCatalogF = new Field<string> { Set = catalogLetter };

        }

        public Document(string filePath, string catalogLetter, string songLyrics)
            :this(filePath,catalogLetter)
        {
            
            Lyrics = new Field<string> {Set = songLyrics};
        }
        [JsonProperty("file_path_id")]
        public string FilePathId { get; set; }
        [JsonProperty("file_path")]
        public Field<string> file_path { get; set; }
        [JsonProperty("extension_f")]
        public Field<string> ExtensionF { get; set; }
        [JsonProperty("letter_catalog_f")]
        public Field<string> LetterCatalogF { get; set; }
        [JsonProperty("Lyrics")]
        public Field<string> Lyrics { get; set; }
    }
}
