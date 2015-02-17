using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sciendo.Lyrics.Common
{
    public interface ILyricsDeserializer
    {
        T Deserialize<T>(string xmlString) where T : class;
        T DeserializeOneFromFile<T>(string fileName) where T : class;
    }

    public class LyricsDeserializer : ILyricsDeserializer
    {
        public virtual T Deserialize<T>(string xmlString) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(new UTF8Encoding().GetBytes(xmlString));
            return xmlSerializer.Deserialize(ms) as T;
        }

        public virtual T DeserializeOneFromFile<T>(string fileName) where T : class
        {
            if (!File.Exists(fileName))
                return null;
            using (TextReader fs = File.OpenText(fileName))
            {
                var str= fs.ReadToEnd();
                if (str.IndexOf(@"<lyrics>Not found</lyrics>") > 0 || str.IndexOf("<html xmlns=\"http://www.w3.org/1999/xhtml")>=0)
                    return null;
                str=str.Replace("\0", "");
                return Deserialize<T>(str);
            }
        }

    }
}
