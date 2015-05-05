using System;

namespace Sciendo.Music.Contracts.MusicService
{
    public class WorkingSet
    {
        public Type MusicFilesProcessorType { get; set; }
        public Type MusicToLyricsFilesProcessorType { get; set; }
        public Type LyricsFilesProcessorType { get; set; } 

    }
}
