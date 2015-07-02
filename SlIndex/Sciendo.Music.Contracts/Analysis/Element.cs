namespace Sciendo.Music.Contracts.Analysis
{
    public partial class Element
    {
        public int ElementId { get; set; }

        public string Name { get; set; }

        public LyricsFileFlag LyricsFileFlag { get; set; }

        public MusicFileFlag MusicFileFlag { get; set; }

        public IndexedFlag IndexedFlag { get; set; }

        public int SnapshotId { get; set; }
    }
}
