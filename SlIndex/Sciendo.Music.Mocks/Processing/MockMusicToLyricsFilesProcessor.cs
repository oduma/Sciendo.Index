using System;
using System.Collections.Generic;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Real.Procesors.MusicSourced;

namespace Sciendo.Music.Mocks.Processing
{
    public class MockMusicToLyricsFilesProcessor: MusicToLyricsFilesProcessor
    {
        public MockMusicToLyricsFilesProcessor()
        {
            LoggingManager.Debug("Constructing Mock Music to Lyrics file processor...");

            WebClient = new MockDownloader();
            LyricsDeserializer = new MockLyricsDeserializer();
            LoggingManager.Debug("Mock Music to Lyrics file processor constructed.");

        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<SongInfo, string,T> specfifcTranformMethod)
        {
            LoggingManager.Debug("Mock Music To Lyrics processor preparing documents...");
            return files.Select(file => specfifcTranformMethod(GetASongInfo(file), file));
        }

        private SongInfo GetASongInfo(string file)
        {
            var songInfo = new SongInfo();
            songInfo.Artists = new[] { "my artist" };
            if (file.Contains(".ogg"))
                songInfo.Title = "not found";
            else if (file.Contains("Processing"))
                songInfo.Title = "needs post procesisng";
            else
                songInfo.Title = "my song";
            return songInfo;
        }
    }
}
