using System;
using Sciendo.Music.Contracts.Common;
using System.Runtime.Serialization;

namespace Sciendo.Music.Contracts.MusicService
{
    [DataContract]
    public class ProgressStatus
    {
        public ProgressStatus()
        {
            MessageCreationDateTime = DateTime.Now;
        }
        [DataMember]
        public string Package { get; set; }
        [DataMember]
        public Status Status { get; set; }
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public DateTime MessageCreationDateTime { get; set; }
    }
}
