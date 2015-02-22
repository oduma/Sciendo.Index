using System;
using System.IO;
using System.Linq;
using System.Net;
using Sciendo.Lyrics.Common;

namespace Sciendo.Lyrics.Provider
{
    public class Pipeline
    {
        internal ExecutionContext ExecutionContext { get; set; }

        private string _executionContextFilePath;

        public Pipeline(string sourceRootDirectory, string targetRootDirectory, string executionContextFilePath)
        {
            if(string.IsNullOrEmpty(executionContextFilePath))
                throw new ArgumentNullException("executionContextFilePath","missing parameter");
            _executionContextFilePath = executionContextFilePath;
            if (File.Exists(_executionContextFilePath))
            {
                ExecutionContext =
                    Sciendo.Common.Serialization.Serializer.DeserializeOneFromFile<ExecutionContext>(
                        _executionContextFilePath);
            }
            else if(!string.IsNullOrEmpty(sourceRootDirectory) 
                && !string.IsNullOrEmpty(targetRootDirectory) 
                && Directory.Exists(sourceRootDirectory) 
                && Directory.Exists(targetRootDirectory))
            {
                ExecutionContext = new ExecutionContext(sourceRootDirectory, targetRootDirectory);
            }
            else
            {
                throw new Exception("Execution Context cannot be established.");
            }
        }

        public static ReadWriteContext ReadWriteContextProvider(string filePath)
        {
            return new ReadWriteContext {ReadLocation = filePath, Status = Status.NotStarted};
        }

        public Pipeline ContinueProcessing(bool retryFailed, Func<string, ReadWriteContext> readWriteContextProvider,Action<Status,string,string> progressEvent)
        {
            if (ExecutionContext == null)
            {
                throw new Exception("Execution Context not established.");
            }
            var webClient = new WebClient();
            if (retryFailed)
            {
                foreach (var readWriteContext in ExecutionContext.ReadWrites.Where(rw => rw.Status != Status.LyricsDownloadedOk))
                {
                    readWriteContext.Progress = progressEvent;
                    readWriteContext.ProcessFile(ReadWriteContext.DefaultMp3FileLoader)
                        .TakeFromWeb(webClient,ExecutionContext.SourceRootDirectory,ExecutionContext.TargetRootDirectory);
                    SaveTrace();
                }
            }
            foreach (
                var filePath in
                    Directory.GetFiles(ExecutionContext.SourceRootDirectory, "*.mp3", SearchOption.AllDirectories)
                        .Where(f => ExecutionContext.ReadWrites.All(rw => rw.ReadLocation != f)))
            {
                var newReadWriteContext=readWriteContextProvider(filePath);
                newReadWriteContext.Progress=progressEvent;
                ExecutionContext.ReadWrites.Add(newReadWriteContext.ProcessFile(ReadWriteContext.DefaultMp3FileLoader).TakeFromWeb(webClient,ExecutionContext.SourceRootDirectory,ExecutionContext.TargetRootDirectory));
                SaveTrace();
            }
            return this;
        }

        public string SaveTrace()
        {
            Sciendo.Common.Serialization.Serializer.SerializeOneToFile(ExecutionContext,_executionContextFilePath);
            return _executionContextFilePath;
        }
    }
}
