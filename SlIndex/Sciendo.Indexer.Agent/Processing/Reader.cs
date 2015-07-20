using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Real.Feedback;

namespace Sciendo.Music.Agent.Processing
{
    public class Reader
    {
        public void ParsePath(string path, string searchPattern,ProcessType processType=ProcessType.None)
        {
            LoggingManager.Debug("Starting parsing path " + path +" for " + searchPattern);
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if(string.IsNullOrEmpty(searchPattern))
                throw new ArgumentNullException("searchPattern");
            CurrentIndexingActivity.Instance.SetAndBroadcast(path, ActivityStatus.InProgress);
            if (processType == ProcessType.Delete)
            {
                LoggingManager.Debug("Attempting to delete " + path);
                ProcessFiles(new[] {path});
                LoggingManager.Debug("Deleted " +path);
            }
            else
            {
                if (Directory.Exists(path))
                {
                    LoggingManager.Debug("Path is a directory...");
                    Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                        .ToList()
                        .ForEach(s => ProcessFiles(GetFiles(s, searchPattern, SearchOption.TopDirectoryOnly)));
                    //Search the current directory also
                    ProcessFiles(GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly));
                }
                else if (File.Exists(path))
                {
                    LoggingManager.Debug("Path is a file...");
                    ProcessFiles(new[] { path });
                }
                else
                    throw new ArgumentException("Invalid path");
                LoggingManager.Debug("Path parsed.");
            }
        }

        internal static IEnumerable<string> GetFiles(string sourceFolder, string filters, SearchOption searchOption,bool includeDirectories=false)
        {
            return (includeDirectories)
                ? (filters.Split('|')
                    .SelectMany(filter => Directory.GetFiles(sourceFolder, filter, searchOption))).Union(
                        Directory.GetDirectories(sourceFolder, "*", searchOption))
                : (filters.Split('|')
                    .SelectMany(filter => Directory.GetFiles(sourceFolder, filter, searchOption)));
        }

        public Action<IEnumerable<string>> ProcessFiles { private get; set; }
    }
}
