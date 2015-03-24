using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Monitoring;

namespace Sciendo.Music.Agent.Processing
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

        public void ParsePath(string path, string searchPattern,ProcessType processType)
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
                    .ForEach(s => ProcessFiles(GetFiles(s, searchPattern, SearchOption.TopDirectoryOnly), _progressEvent,processType));
                //Search the current directory also
                ProcessFiles(GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly), _progressEvent, processType);
            }
            else if (File.Exists(path))
            {
                LoggingManager.Debug("Path is a file...");
                ProcessFiles(new string[] { path }, _progressEvent,processType);   
            }
            else 
                throw new ArgumentException("Invalid path");
            LoggingManager.Debug("Path parsed.");
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

        public Action<IEnumerable<string>, Action<Status,string>,ProcessType> ProcessFiles { private get; set; }
    }
}
