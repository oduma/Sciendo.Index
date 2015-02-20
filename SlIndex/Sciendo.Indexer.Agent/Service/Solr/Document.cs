using System;
using System.IO;

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
            file_path_id = filePath.ToLower();
            file_path = new Field<string> { set = Prefix + filePath };

            extension_f = new Field<string> { set = Path.GetExtension(file_path_id).Replace(".", "") };
            letter_catalog_f = new Field<string> { set = catalogLetter };

        }

        public Document(string filePath, string catalogLetter, string songLyrics)
            :this(filePath,catalogLetter)
        {
            
            lyrics = new Field<string> {set = songLyrics};
        }
        public string file_path_id { get; set; }
        public Field<string> file_path { get; set; }
        public Field<string> extension_f { get; set; }
        public Field<string> letter_catalog_f { get; set; }
        public Field<string> lyrics { get; set; }
    }
}
