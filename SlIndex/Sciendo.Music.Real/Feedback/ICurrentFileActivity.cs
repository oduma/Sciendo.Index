using System;
namespace Sciendo.Music.Real.Feedback
{
    public interface ICurrentFileActivity
    {
        ActivityStatus ActivityStatus { get; }
        void BroadcastDetails(string details);
        void ClearAndBroadcast();
        string Details { get; }
        string FromPath { get; }
        void SetAndBroadcast(string fromPath, ActivityStatus activityStatus);
        string ToString();
    }
}
