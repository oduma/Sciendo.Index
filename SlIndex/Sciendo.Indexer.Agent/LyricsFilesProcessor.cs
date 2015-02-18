﻿using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Sciendo.Common.Logging;
using Sciendo.Index.Solr;
using Sciendo.Lyrics.Common;
using System;

namespace Sciendo.Indexer.Agent
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
            foreach (string file in files)
            {
                var lyricsResult = LyricsDeserializer.DeserializeOneFromFile<LyricsResult>(file);
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
            var musicFile = file.Replace(CurrentConfiguration.SourceDirectory, _musicRootFolder);

            return Directory.GetFiles(Path.GetDirectoryName(musicFile),
                                Path.GetFileNameWithoutExtension(musicFile) + ".*")[0];
        }
    }
}
