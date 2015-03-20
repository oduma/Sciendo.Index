using System;
using Sciendo.Music.Contracts.Common;

namespace Sciendo.Music.Contracts.MusicService
{
    public class ProgressStatus
    {
        public string Package { get; set; }
        public Status Status { get; set; }
        public Guid Id { get; set; }
    }
}
