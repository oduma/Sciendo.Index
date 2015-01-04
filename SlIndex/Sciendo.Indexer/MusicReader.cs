using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Indexer
{
    public class MusicReader:ReaderBase
    {
        public MusicReader()
        {
            SearchPattern = "*.mp3|*.ogg";
        }

        public void RootReader()
        {
            if (string.IsNullOrEmpty(RootDirectory) || !Directory.Exists(RootDirectory))
                throw new ArgumentException("root has to be a directory");
            Directory.GetDirectories(RootDirectory, "*", SearchOption.AllDirectories)
                .ToList()
                .ForEach(s=>ContinueWithDirectory(s));
        }
    }
}
