﻿using System;
using System.Collections.Generic;
using System.Configuration;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Contracts.Solr;
using Sciendo.Music.Mocks.Solr;
using Sciendo.Music.Real.Procesors.Configuration;
using Sciendo.Music.Real.Procesors.MusicSourced;

namespace Sciendo.Music.Mocks.Processing
{
    public class MockIndexingFilesProcessor:IndexingFilesProcessor
    {
        public MockIndexingFilesProcessor()
        {
            LoggingManager.Debug("Constructing MockMusicFilesprocessor...");
            Sender = new MockSender();
            CurrentConfiguration = ((AgentConfigurationSection) ConfigurationManager.GetSection("agent"));
            LoggingManager.Debug("MockMusicFilesprocessor constructed.");
        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<SongInfo, string, T> specificTransfromFunction)
        {
            LoggingManager.Debug("MockMusicFilesprocessor preparing documents...");
            foreach (string file in files)
            {
                if(file==@"c:\users\something\something")
                    DeletedDocuments.Add(new DeleteDocument(file));
                else
            yield return
                specificTransfromFunction(
                    new SongInfo {Album = "test album1", Artists = new[] {"test artist 1"}, Title = "new song 1"},
                    @"c:\\abc\abc\abc.mp3");
            }
            LoggingManager.Debug("MockMusicFilesprocessor documents prrepared.");
        }

    }
}
