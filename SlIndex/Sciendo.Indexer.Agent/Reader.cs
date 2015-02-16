using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent
{
    public class Reader
    {
        private readonly Action<Status, string> _progressEvent;

        public Reader(Action<Status, string> progressEvent)
        {
            _progressEvent = progressEvent;
        }

        public void ParsePath(string path,string searchPattern)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if(Directory.Exists(path))
                Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                    .ToList()
                    .ForEach(s => ContinueWithDirectory(s, searchPattern, path));
            else if (File.Exists(path))
                ProcessFiles(new string[] {path},Path.GetDirectoryName(path), _progressEvent);
        }

        private void ContinueWithDirectory(string directory, string searchPattern, string rootFolder)
        {
            var files = GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);

            if (files.Any())
            {
                ProcessFiles(files, rootFolder, _progressEvent);
            }
        }

        private static IEnumerable<string> GetFiles(string sourceFolder, string filters, SearchOption searchOption)
        {
            return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption));
        }

        public Action<IEnumerable<string>, string, Action<Status,string>> ProcessFiles { private get; set; }
    }
}
