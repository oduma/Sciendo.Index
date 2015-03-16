﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Music.Agent.Common;
using Sciendo.Music.Agent.Service.Solr;
using Sciendo.Music.Agent.Service.Solr.Mocks;

namespace Sciendo.Music.Agent.Processing.Mocks
{
    public class MockMusicFilesProcessor:MusicFilesProcessor
    {
        public MockMusicFilesProcessor()
        {
            LoggingManager.Debug("Constructing MockMusicFilesprocessor...");
            Sender = new MockSender();
            CurrentConfiguration = ((AgentConfigurationSection) ConfigurationManager.GetSection("indexer")).Music;
            LoggingManager.Debug("MockMusicFilesprocessor constructed.");
        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<SongInfo, string, T> specificTransfromFunction)
        {
            LoggingManager.Debug("MockMusicFilesprocessor preparing documents...");
            yield return
                specificTransfromFunction(
                    new SongInfo {Album = "test album1", Artists = new[] {"test artist 1"}, Title = "new song 1"},
                    @"c:\\abc\abc\abc.mp3");
            LoggingManager.Debug("MockMusicFilesprocessor documents prrepared.");
        }

    }
}
