using System.Collections.Generic;

namespace Sciendo.Lyrics.Provider.Service
{
    public class ExecutionContext
    {
        public string SourceRootDirectory { get; set; }
        public string TargetRootDirectory { get; set; }

        public ExecutionContext(string sourceRootDirectory, string targetRootDirectory)
        {
            this.SourceRootDirectory = sourceRootDirectory;
            this.TargetRootDirectory = targetRootDirectory;
            ReadWrites= new List<ReadWriteContext>();
        }

        public ExecutionContext()
        {
            ReadWrites = new List<ReadWriteContext>();
        }

        public ExecutionContext(string sourceRootDirectory, string targetRootDirectory, string searchPattern):this(sourceRootDirectory, targetRootDirectory)
        {
            SearchPattern = searchPattern;
        }

        public List<ReadWriteContext> ReadWrites { get; set; }
        public string SearchPattern { get; set; }
    }
}
