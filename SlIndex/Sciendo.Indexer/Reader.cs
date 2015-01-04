using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sciendo.Indexer
{
    public class Reader
    {
        public void ParseDirectory(string directory,string searchPattern)
        {
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
                throw new ArgumentException("root has to be a directory");
            Directory.GetDirectories(directory, "*", SearchOption.AllDirectories)
                .ToList()
                .ForEach(s => ContinueWithDirectory(s, searchPattern, directory));
        }

        private void ContinueWithDirectory(string directory, string searchPattern, string rootFolder)
        {
            var files = GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);

            if (files.Any())
            {
                ProcessFiles(files, rootFolder);
            }
        }

        private static IEnumerable<string> GetFiles(string sourceFolder, string filters, SearchOption searchOption)
        {
            return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption));
        }

        public Action<IEnumerable<string>, string> ProcessFiles { private get; set; }
    }
}
