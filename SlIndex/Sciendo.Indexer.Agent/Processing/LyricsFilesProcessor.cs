using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Sciendo.Common.Logging;
using Sciendo.Indexer.Agent.Service.Solr;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent.Processing
{
    public class LyricsFilesProcessor:FilesProcessor
    {
        protected ILyricsDeserializer LyricsDeserializer { private get; set; }
        private readonly string _musicRootFolder;

        public LyricsFilesProcessor()
        {
            LoggingManager.Debug("Constructing LyricsFilesprocessor...");
            var configSection = (IndexerConfigurationSection) ConfigurationManager.GetSection("indexer");
            Sender=new SolrSender(configSection.SolrConnectionString);
            CurrentConfiguration = configSection.Lyrics;
            _musicRootFolder = configSection.Music.SourceDirectory;
            LyricsDeserializer = new LyricsDeserializer();
            LoggingManager.Debug("LyricsFilesprocessor constructed.");
        }

        public LyricsFilesProcessor(IndexerConfigurationSource givenConfig, string musicRootFolder)
        {
            _musicRootFolder = musicRootFolder;
            CurrentConfiguration = givenConfig;
        }

        protected override IEnumerable<Document> PrepareDocuments(IEnumerable<string> files)
        {
            LoggingManager.Debug("LyricsFileProcessor preparing documents...");
            foreach (string file in files)
            {
                var lyricsResult = LyricsDeserializer.Deserialize<LyricsResult>(file);
                if (lyricsResult != null)
                {
                    var musicFile = GetMusicFile(file);
                    if(!string.IsNullOrEmpty(musicFile))
                    {
                        yield return new Document(musicFile,CatalogLetter(musicFile,_musicRootFolder), lyricsResult.lyrics);
                    }
                    else
                    {
                        throw new Exception("Cannot determine the music file for the lyrics file: " + file);
                    }
                }
            }
        }

        private string GetMusicFile(string file)
        {
            LoggingManager.Debug("Get Music File from lyrics file: " +file);
            LoggingManager.Debug("Get Music Using MusicRootFolder: " + _musicRootFolder);
            var musicFile = file.Replace(CurrentConfiguration.SourceDirectory, _musicRootFolder);

            musicFile = Directory.GetFiles(Path.GetDirectoryName(musicFile),
                                Path.GetFileNameWithoutExtension(musicFile) + ".*")[0];
            LoggingManager.Debug("Music file: "+musicFile);
            return musicFile;
        }
    }
}
