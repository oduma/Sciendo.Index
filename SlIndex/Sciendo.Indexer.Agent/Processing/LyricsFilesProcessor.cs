﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Sciendo.Common.Logging;
using Sciendo.Lyrics.Common;
using Sciendo.Music.Agent.Service.Solr;

namespace Sciendo.Music.Agent.Processing
{
    public class LyricsFilesProcessor:FilesProcessor<string>
    {
        protected ILyricsDeserializer LyricsDeserializer { private get; set; }
        private readonly string _musicRootFolder;

        public LyricsFilesProcessor()
        {
            LoggingManager.Debug("Constructing LyricsFilesprocessor...");
            var configSection = (AgentConfigurationSection) ConfigurationManager.GetSection("agent");
            Sender=new SolrSender(configSection.SolrConnectionString);
            CurrentConfiguration = configSection.Lyrics;
            _musicRootFolder = configSection.Music.SourceDirectory;
            LyricsDeserializer = new LyricsDeserializer();
            LoggingManager.Debug("LyricsFilesprocessor constructed.");
        }

        public LyricsFilesProcessor(AgentConfigurationSource givenConfig, string musicRootFolder)
        {
            _musicRootFolder = musicRootFolder;
            CurrentConfiguration = givenConfig;
        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<string, string, T> specificTransfromFunction)
        {
            LoggingManager.Debug("LyricsFileProcessor preparing documents...");
            foreach (string file in files)
            {
                var lyricsResult = LyricsDeserializer.DeserializeFromFile<LyricsResult>(file);
                if (lyricsResult != null)
                {
                    var musicFile = GetMusicFile(file);
                    if(!string.IsNullOrEmpty(musicFile))
                    {
                        yield return specificTransfromFunction(lyricsResult.lyrics, musicFile);
                    }
                    else
                    {
                        throw new Exception("Cannot determine the music file for the Lyrics file: " + file);
                    }
                }
            }
        }

        private string GetMusicFile(string file)
        {
            LoggingManager.Debug("Get Music File from Lyrics file: " +file);
            LoggingManager.Debug("Get Music Using MusicRootFolder: " + _musicRootFolder);
            var musicFile = file.Replace(CurrentConfiguration.SourceDirectory, _musicRootFolder);

            musicFile = Directory.GetFiles(Path.GetDirectoryName(musicFile),
                                Path.GetFileNameWithoutExtension(musicFile) + ".*")[0];
            LoggingManager.Debug("Music file: "+musicFile);
            return musicFile;
        }

        public string CreateLyricsFolder(string musicFolder)
        {
            LoggingManager.Debug("Create or Lirycs path from music path:" + musicFolder);
            var lyricsFolder = musicFolder.Replace(_musicRootFolder, CurrentConfiguration.SourceDirectory);
            if (!Directory.Exists(lyricsFolder))
                Directory.CreateDirectory(lyricsFolder);
            return lyricsFolder;

        }

        protected override Document TransformToDocument(string transfromFrom, string file)
        {
            return new Document(file, CatalogLetter(file, _musicRootFolder), transfromFrom);
        }
    }
}
