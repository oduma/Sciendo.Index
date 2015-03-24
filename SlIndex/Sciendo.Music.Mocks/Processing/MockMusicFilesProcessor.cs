using System;
using System.Collections.Generic;
using System.Configuration;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Mocks.Solr;
using Sciendo.Music.Real.Procesors.Configuration;
using Sciendo.Music.Real.Procesors.MusicSourced;

namespace Sciendo.Music.Mocks.Processing
{
    public class MockMusicFilesProcessor:MusicFilesProcessor
    {
        public MockMusicFilesProcessor()
        {
            LoggingManager.Debug("Constructing MockMusicFilesprocessor...");
            Sender = new MockSender();
            CurrentConfiguration = ((AgentConfigurationSection) ConfigurationManager.GetSection("agent")).Music;
            LoggingManager.Debug("MockMusicFilesprocessor constructed.");
        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<SongInfo, string, ProcessType, T> specificTransfromFunction, ProcessType processType)
        {
            LoggingManager.Debug("MockMusicFilesprocessor preparing documents...");
            yield return
                specificTransfromFunction(
                    new SongInfo {Album = "test album1", Artists = new[] {"test artist 1"}, Title = "new song 1"},
                    @"c:\\abc\abc\abc.mp3",processType);
            LoggingManager.Debug("MockMusicFilesprocessor documents prrepared.");
        }

    }
}
