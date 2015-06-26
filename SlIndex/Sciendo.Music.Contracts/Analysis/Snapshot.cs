using System;
namespace Sciendo.Music.Contracts.Analysis
{

    public partial class Snapshot
    {
        public int SnapshotId { get; set; }

        public DateTime TakenAt { get; set; }

        public string Name { get; set; }
    }
}
