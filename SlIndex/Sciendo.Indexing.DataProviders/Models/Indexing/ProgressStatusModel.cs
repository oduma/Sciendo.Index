using System;

namespace Sciendo.Music.DataProviders.Models.Indexing
{
    public class ProgressStatusModel
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Package { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
