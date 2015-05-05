using System;
using Sciendo.Music.Contracts.Common;

namespace Sciendo.Music.Contracts.MusicService
{
    public class ProgressStatus
    {
        public ProgressStatus()
        {
            MessageCreationDateTime = DateTime.Now;
        }
        public string Package { get; set; }
        public Status Status { get; set; }
        public Guid Id { get; set; }
        public DateTime MessageCreationDateTime { get; set; }
    }
}
