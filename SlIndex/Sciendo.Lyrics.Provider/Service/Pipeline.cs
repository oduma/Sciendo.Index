using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sciendo.Common.Serialization;
using Sciendo.Lyrics.Common;

namespace Sciendo.Lyrics.Provider.Service
{
    public class Pipeline
    {
        internal ExecutionContext ExecutionContext { get; set; }

        private readonly string _executionContextFilePath;
        private readonly WebDownloaderBase _webClient;

        public Pipeline(string sourceRootDirectory, 
            string targetRootDirectory, 
            WebDownloaderBase webClient, 
            string executionContextFilePath=null)
        {
            if(webClient==null)
                throw new ArgumentNullException("webClient");
            _webClient = webClient;
            _executionContextFilePath = (string.IsNullOrEmpty(executionContextFilePath)) ? "executionContext.xml" : executionContextFilePath;
            if (File.Exists(_executionContextFilePath))
            {
                ExecutionContext =
                    Serializer.DeserializeOneFromFile<ExecutionContext>(
                        _executionContextFilePath);
            }
            else if (!string.IsNullOrEmpty(sourceRootDirectory)
                     && !string.IsNullOrEmpty(targetRootDirectory)
                     && Directory.Exists(sourceRootDirectory)
                     && Directory.Exists(targetRootDirectory))
            {
                ExecutionContext = new ExecutionContext(sourceRootDirectory, targetRootDirectory,"*.mp3|*.ogg");
            }
            else
            {
                throw new NoExecutionContextException("Execution Context cannot be established.");
            }
        }

        public static ReadWriteContext ReadWriteContextProvider(string filePath)
        {
            return new ReadWriteContext {ReadLocation = filePath, Status = Status.None};
        }

        public Pipeline ContinueProcessing(bool retryFailed, Func<string, ReadWriteContext> readWriteContextProvider, Action<Status, string, string> progressEvent, ILyricsDeserializer lyricsDeserializer)
        {
            if (ExecutionContext == null)
            {
                throw new NoExecutionContextException("Execution Context cannot be established.");
            }
            if (retryFailed)
            {
                foreach (var readWriteContext in ExecutionContext.ReadWrites.Where(rw => rw.Status != Status.LyricsDownloadedOk))
                {
                    readWriteContext.Progress = progressEvent;
                    readWriteContext.ProcessFile(ReadWriteContext.DefaultMp3FileLoader)
                        .TakeFromWeb(_webClient,ExecutionContext.SourceRootDirectory,ExecutionContext.TargetRootDirectory, lyricsDeserializer);
                    SaveTrace();
                }
            }
            foreach (
                var filePath in
                    GetFiles(ExecutionContext.SourceRootDirectory, ExecutionContext.SearchPattern, SearchOption.AllDirectories)
                        .Where(f => ExecutionContext.ReadWrites.All(rw => rw.ReadLocation != f)))
            {
                var newReadWriteContext=readWriteContextProvider(filePath);
                newReadWriteContext.Progress=progressEvent;
                ExecutionContext.ReadWrites.Add(newReadWriteContext.ProcessFile(ReadWriteContext.DefaultMp3FileLoader)
                    .TakeFromWeb(_webClient,ExecutionContext.SourceRootDirectory,ExecutionContext.TargetRootDirectory, lyricsDeserializer));
                SaveTrace();
            }
            return this;
        }

        private static IEnumerable<string> GetFiles(string sourceFolder, string filters, SearchOption searchOption)
        {
            return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption));
        }

        public string SaveTrace()
        {
            Sciendo.Common.Serialization.Serializer.SerializeOneToFile(ExecutionContext,_executionContextFilePath);
            return _executionContextFilePath;
        }
    }
}
