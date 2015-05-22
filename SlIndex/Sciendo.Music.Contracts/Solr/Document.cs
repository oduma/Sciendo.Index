﻿using System;
using System.IO;
using Newtonsoft.Json;

namespace Sciendo.Music.Contracts.Solr
{
    public class Document
    {
        private const string Prefix = "file:///";
        [JsonProperty("artist")]
        public Field<string[]> Artist { get; set; }
        [JsonProperty("title")]
        public Field<string> Title { get; set; }
        [JsonProperty("album")]
        public Field<string> Album { get; set; }

        public Document (string filePath, string catalogLetter)
        [JsonProperty("letter_catalog_f")]
        public Field<string> LetterCatalogF { get; set; }
        [JsonProperty("lyrics")]
        public Field<string> Lyrics { get; set; }

        public Document(string filePath, string catalogLetter, string[] artists, string song, string albumName)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");
            FilePathId = filePath.ToLower();
            file_path = new Field<string> { Set = Prefix + filePath };

            ExtensionF = new Field<string> { Set = Path.GetExtension(FilePathId).Replace(".", "") };
            LetterCatalogF = new Field<string> { Set = catalogLetter };

            Artist = new Field<string[]> { Set = artists };
            Title = new Field<string> { Set = song };
            Album = new Field<string> { Set = albumName };
        }

        public Document(string filePath, string catalogLetter, string songLyrics)
        {
            Lyrics =  new Field<string> {Set = songLyrics};
        }
        [JsonProperty("file_path_id")]
        public string FilePathId { get; set; }
        [JsonProperty("file_path")]
        public Field<string> file_path { get; set; }
    }
}
