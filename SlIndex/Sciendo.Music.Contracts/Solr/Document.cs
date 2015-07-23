using System;
using System.IO;
using Newtonsoft.Json;

namespace Sciendo.Music.Contracts.Solr
{
    public class Document
    {
        private const string Prefix = "file:///";

        public Document()
        {
        }

        [JsonProperty("artist")]
        public Field<string[]> Artist { get; set; }

        [JsonProperty("title")]
        public Field<string> Title { get; set; }

        [JsonProperty("album")]
        public Field<string> Album { get; set; }

        [JsonProperty("file_path_id")]
        public string FilePathId { get; set; }

        [JsonProperty("file_path")]
        public Field<string> FilePath { get; set; }

        [JsonProperty("extension_f")]
        public Field<string> ExtensionF { get; set; }

        [JsonProperty("letter_catalog_f")]
        public Field<string> LetterCatalogF { get; set; }

        [JsonProperty("lyrics")]
        public Field<string> Lyrics { get; set; }

        public Document(string filePath, string catalogLetter, string[] artists, string song, string albumName)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");
            FilePath = new Field<string>{Set=Prefix + filePath};
            FilePathId = filePath;
            var extension = Path.GetExtension(FilePathId);
            ExtensionF = new Field<string> {Set = extension.Replace(".", "")};
            LetterCatalogF = new Field<string> {Set = catalogLetter};

            Artist = new Field<string[]> {Set = artists};
            Title = new Field<string> {Set = song};
            Album = new Field<string> {Set = albumName};
        }

        public Document(string filePath, string catalogLetter, string[] artists, string songTitle, string albumName,
            string lyrics) : this(filePath, catalogLetter, artists, songTitle, albumName)
        {
            Lyrics = new Field<string> {Set = lyrics};
        }
    }
}