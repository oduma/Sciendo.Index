using Sciendo.Common.Logging;

namespace Sciendo.Lyrics.Common
{
    public class MockLyricsDeserializer:ILyricsDeserializer
    {
        public T Deserialize<T>(string xmlString) where T : class
        {
            throw new System.NotImplementedException();
        }

        public T DeserializeOneFromFile<T>(string fileName) where T : class
        {
            LoggingManager.Debug("Mock Deserializing lyrics from file: " +fileName);
            return new LyricsResult { lyrics = "test lyrics" } as T;
        }

    }
}
