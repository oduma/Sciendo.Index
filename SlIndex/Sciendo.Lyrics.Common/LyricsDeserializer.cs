using Sciendo.Common.Logging;
using Sciendo.Common.Serialization;

namespace Sciendo.Lyrics.Common
{
    public interface ILyricsDeserializer
    {
        T Deserialize<T>(string fileName) where T : class;
    }

    public class LyricsDeserializer : ILyricsDeserializer
    {

        public virtual T Deserialize<T>(string fileName) where T : class
        {
            LoggingManager.Debug("Deserializing lyrics from file: " +fileName);
            return Serializer.DeserializeOneFromFile<T>(fileName, LyricsNotEmpty, LyricsFix);
        }

        private string LyricsFix(string arg)
        {
            return arg.Replace("\0", "");
        }

        private bool LyricsNotEmpty(string arg)
        {
            return !(arg.IndexOf(@"<lyrics>Not found</lyrics>") > 0 ||
             arg.IndexOf("<html xmlns=\"http://www.w3.org/1999/xhtml") >= 0);
        }
    }
}
