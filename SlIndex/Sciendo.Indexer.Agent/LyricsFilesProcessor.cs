using System.Collections.Generic;
using System.IO;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent
{
    public class LyricsFilesProcessor:FilesProcessor
    {
        private string _musicRootDirectory;

        public LyricsFilesProcessor(string musicRootDirectory, SolrSender solrSender):base(solrSender)
        {
            _musicRootDirectory = musicRootDirectory;
        }

        protected override IEnumerable<Document> PrepareDocuments(IEnumerable<string> files, string rootFolder)
        {
            foreach (string file in files)
            {
                var lyricsResult = LyricsDeserializer.DeserializeOneFromFile<LyricsResult>(file);
                if (lyricsResult != null)
                {
                    var musicFile = file.Replace(rootFolder, _musicRootDirectory);
                    musicFile = Directory.GetFiles(Path.GetDirectoryName(musicFile), Path.GetFileNameWithoutExtension(musicFile) + ".*")[0];
                    yield return new Document(musicFile, _musicRootDirectory, lyricsResult.lyrics);
                }
            }
        }
    }
}
