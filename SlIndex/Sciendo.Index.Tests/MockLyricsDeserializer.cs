using Sciendo.Lyrics.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Index.Tests
{
    public class MockLyricsDeserializer:LyricsDeserializer
    {
        public override T DeserializeOneFromFile<T>(string fileName)
        {
            return new LyricsResult { lyrics = "test lyrics" } as T;
        }
    }
}
