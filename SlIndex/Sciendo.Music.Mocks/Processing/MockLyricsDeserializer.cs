using System.IO;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Real;
using Sciendo.Music.Real.Lyrics.Provider;

namespace Sciendo.Music.Mocks.Processing
{
    public class MockLyricsDeserializer:LyricsDeserializer
    {
        public override T DeserializeFromFile<T>(string fileName) 
        {
            LoggingManager.Debug("Mock Deserializing lyrics from file: " +fileName);
            return new LyricsResult { lyrics = "test lyrics" } as T;
        }

        public override T Deserialize<T>(string xmlString) 
        {
            if (xmlString == "no lyrics")
                return base.Deserialize<T>(File.ReadAllText("ExampleNoLyrics.xml")) as T;
            if (xmlString == "post processing")
                return base.Deserialize<T>(File.ReadAllText("ExampleNeedsPostProcessing.xml"));
            return new LyricsResult {lyrics = "my lyrircs"} as T;
        }
    }
}
