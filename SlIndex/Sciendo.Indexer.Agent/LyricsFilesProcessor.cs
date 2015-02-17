using System.Collections.Generic;
using System.IO;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;
using System;

namespace Sciendo.Indexer.Agent
{
    public class LyricsFilesProcessor:FilesProcessor
    {
        private string _musicPath;
        private LyricsDeserializer _lyricsDeserializer;

        public LyricsFilesProcessor(string musicPath, SolrSender solrSender, LyricsDeserializer lyricsDeserializer):base(solrSender)
        {
            if (string.IsNullOrEmpty(musicPath))
                throw new ArgumentNullException("musicPath");
            if (!Directory.Exists(musicPath) && !File.Exists(musicPath))
                throw new ArgumentException("Invalid path");
            _musicPath = musicPath;
            _lyricsDeserializer = lyricsDeserializer;
        }

        protected override IEnumerable<Document> PrepareDocuments(IEnumerable<string> files, string rootFolder)
        {
            foreach (string file in files)
            {
                var lyricsResult = _lyricsDeserializer.DeserializeOneFromFile<LyricsResult>(file);
                if (lyricsResult != null)
                {
                    var musicFile = file.Replace(rootFolder, _musicPath);
                    musicFile = Directory.GetFiles(Path.GetDirectoryName(musicFile), Path.GetFileNameWithoutExtension(musicFile) + ".*")[0];
                    yield return new Document(musicFile, _musicPath, lyricsResult.lyrics);
                }
            }
        }
    }
}
