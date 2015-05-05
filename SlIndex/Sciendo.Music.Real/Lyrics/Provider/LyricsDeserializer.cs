using Sciendo.Common.Logging;
using Sciendo.Common.Serialization;
using Sciendo.Music.Contracts.Processing;

namespace Sciendo.Music.Real.Lyrics.Provider
{

    public class LyricsDeserializer : ILyricsDeserializer
    {

        public virtual T DeserializeFromFile<T>(string fileName) where T : class
        {
            LoggingManager.Debug("Deserializing lyrics from file: " +fileName);
            return Serializer.DeserializeOneFromFile<T>(fileName, LyricsNotEmpty, LyricsFix);
        }

        public virtual T Deserialize<T>(string xmlString) where T : class
        {
            LoggingManager.Debug("Deserializing lyrics from string.");
            return Serializer.Deserialize<T>(xmlString, LyricsNotEmpty, LyricsFix);

        }

        private static string LyricsFix(string arg)
        {
            return arg.Replace("\0", "");
        }

        private static bool LyricsNotEmpty(string arg)
        {
            return !(arg.IndexOf(@"<lyrics>Not found</lyrics>") > 0 ||
             arg.IndexOf("<html xmlns=\"http://www.w3.org/1999/xhtml") >= 0);
        }
    }
}
