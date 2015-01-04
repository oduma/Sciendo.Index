using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sciendo.Indexer
{
    public abstract class ReaderBase
    {
        public string RootDirectory { get; set; }

        protected string SearchPattern;


        protected void ContinueWithDirectory(string directory)
        {
            var files = GetFiles(directory, SearchPattern, SearchOption.TopDirectoryOnly);

            if (files.Any())
            {
                ProcessFiles(files);
            }
        }

        private static IEnumerable<string> GetFiles(string sourceFolder, string filters, SearchOption searchOption)
        {
            return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption));
        }

        public Action<IEnumerable<string>> ProcessFiles { private get; set; }
    }
}
