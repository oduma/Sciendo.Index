using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Lyrics.Common;

namespace Sciendo.Indexer.Agent.Processing
{
    public class Reader
    {
        private readonly Action<Status, string> _progressEvent;

        public Reader(Action<Status, string> progressEvent)
        {
            LoggingManager.Debug("Constructing reader...");
            _progressEvent = progressEvent;
            LoggingManager.Debug("Reader constructed");
        }

        public void ParsePath(string path, string searchPattern)
        {
            LoggingManager.Debug("Starting parsing path " + path +" for " + searchPattern);
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if(string.IsNullOrEmpty(searchPattern))
                throw new ArgumentNullException("searchPattern");
            if(Directory.Exists(path))
            {
                LoggingManager.Debug("Path is a directory...");
                Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                    .ToList()
                    .ForEach(s => ContinueWithDirectory(s, searchPattern));
                //Search the current directory also
                ContinueWithDirectory(path, searchPattern);
            }
            else if (File.Exists(path))
            {
                LoggingManager.Debug("Path is a file...");
                ProcessFiles(new string[] { path }, _progressEvent);   
            }
            else 
                throw new ArgumentException("Invalid path");
            LoggingManager.Debug("Path parsed.");
        }

        private void ContinueWithDirectory(string directory, string searchPattern)
        {
            LoggingManager.Debug("Parsing Directory: "+directory +" for: " +searchPattern);
            var files = GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);

            if (files.Any())
            {
                ProcessFiles(files, _progressEvent);
            }
            LoggingManager.Debug("Directory parsed.");
        }

        internal static IEnumerable<string> GetFiles(string sourceFolder, string filters, SearchOption searchOption,bool includeDirectories=false)
        {
            return (includeDirectories)
                ? (filters.Split('|')
                    .SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption))).Union(
                        Directory.GetDirectories(sourceFolder, "*", searchOption))
                : (filters.Split('|')
                    .SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption)));
        }

        public Action<IEnumerable<string>, Action<Status,string>> ProcessFiles { private get; set; }
    }
}
