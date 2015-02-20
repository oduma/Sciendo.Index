﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Id3;
using Id3.Id3;
using Sciendo.Common.Logging;
using Sciendo.Indexer.Agent.Service.Solr;

namespace Sciendo.Indexer.Agent.Processing
{
    public class MusicFilesProcessor:FilesProcessor
    {
        public MusicFilesProcessor()
        {
            LoggingManager.Debug("Constructing MusicFilesProcessor...");
            var configSection = (IndexerConfigurationSection) ConfigurationManager.GetSection("indexer");
            Sender=new SolrSender(configSection.SolrConnectionString);
            CurrentConfiguration = configSection.Music;
            LoggingManager.Debug("MusicFilesProcessor constructed.");
        }

        protected override IEnumerable<Document> PrepareDocuments(IEnumerable<string> files)
        {
            LoggingManager.Debug("MusicFileProcessor preparing documents...");
            string[] artists = null;
            string title=string.Empty;
            string album=string.Empty;

            foreach (var file in files)
            {
                IMp3Stream mp3File = new Mp3File(file);
                if (mp3File.HasTags && mp3File.AvailableTagVersions != null)
                {
                    var version = Enumerable.FirstOrDefault<Version>(mp3File.AvailableTagVersions);
                    if (version != null)
                    {
                        IId3Tag id3Tag=null;
                        try
                        {
                            id3Tag= mp3File.GetTag(version.Major, version.Minor);
                        }
                        catch { }
                        if (id3Tag != null)
                        {
                            artists = Enumerable.Select<string, string>(id3Tag.Artists.Value, a=>string.Join("",Enumerable.Where<char>(a.ToCharArray(), c=>((int)c)>=32))).ToArray();
                            title = id3Tag.Title.TextValue;
                            album = id3Tag.Album.TextValue;
                        }
                    }
                }
        
                yield return new FullDocument(file, CatalogLetter(file,CurrentConfiguration.SourceDirectory),artists,title,album);
            }
            LoggingManager.Debug("MusicFileProcessor documents prepared.");
        }

    }
}