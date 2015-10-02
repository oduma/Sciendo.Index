using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Real.Feedback;
using Sciendo.Music.Real.Procesors.Common;

namespace Sciendo.Music.Agent.Processing
{
    public class Reader
    {
        private ICurrentFileActivity _currentFileActivity;
        public Reader(ICurrentFileActivity currentFileActivity)
        {
            _currentFileActivity = currentFileActivity;
        }
        public void ParsePath(string path, string searchPattern,ProcessType processType=ProcessType.None)
        {
            LoggingManager.Debug("Starting parsing path " + path +" for " + searchPattern);
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if(string.IsNullOrEmpty(searchPattern))
                throw new ArgumentNullException("searchPattern");
            if(_currentFileActivity!=null)
                _currentFileActivity.SetAndBroadcast(path, ActivityStatus.InProgress);
            if (processType == ProcessType.Delete)
            {
                LoggingManager.Debug("Attempting to delete " + path);
                if (_currentFileActivity != null)
                    _currentFileActivity.BroadcastDetails("Deleting: " + path);
                ProcessFiles(new[] { path });
                if (_currentFileActivity != null)
                    _currentFileActivity.SetAndBroadcast(path,ActivityStatus.Stopped);
                LoggingManager.Debug("Deleted " + path);
            }
            else
            {
                if (Directory.Exists(path))
                {
                    LoggingManager.Debug("Path is a directory...");
                    Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                        .ToList()
                        .ForEach(s => {
                                if(_currentFileActivity!=null)
                                    _currentFileActivity.BroadcastDetails("Processing: " + s);
                                    var processResponse = ProcessFiles(GetFiles(s, searchPattern, SearchOption.TopDirectoryOnly));
                                    if (_currentFileActivity != null)
                                        _currentFileActivity.BroadcastDetails("Processed: " + s + " " + processResponse.Status);
                        });
                    //Search the current directory also
                    if(_currentFileActivity!=null)
                        _currentFileActivity.BroadcastDetails("Processing: " + path);
                    var processResponse1 = ProcessFiles(GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly));
                    if (_currentFileActivity != null)
                        _currentFileActivity.BroadcastDetails("Processed: " + path + " " + processResponse1.Status);

                }
                else if (File.Exists(path))
                {
                    LoggingManager.Debug("Path is a file...");
                    if (_currentFileActivity != null)
                        _currentFileActivity.BroadcastDetails("Processing: " + path);
                    ProcessFiles(new[] { path });
                }
                else
                {
                    if (_currentFileActivity != null)
                    {
                        _currentFileActivity.SetAndBroadcast(path, ActivityStatus.Stopped);
                    }
                    throw new ArgumentException("Invalid path");
                }
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

        public Func<IEnumerable<string>,ProcessResponse> ProcessFiles { private get; set; }
    }
}
