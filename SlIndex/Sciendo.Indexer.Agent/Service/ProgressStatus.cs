﻿using System;
using Sciendo.Lyrics.Common;

namespace Sciendo.Music.Agent.Service
{
    public class ProgressStatus
    {
        public string Package { get; set; }
        public Status Status { get; set; }
        public Guid Id { get; set; }
    }
}
