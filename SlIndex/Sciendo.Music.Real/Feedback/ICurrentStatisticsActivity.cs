using System;
namespace Sciendo.Music.Real.Feedback
{
    public interface ICurrentStatisticsActivity
    {
        ActivityStatus ActivityStatus { get; }
        void BroadcastDetails(string details);
        void ClearAndBroadcast();
        string Details { get; }
        void SetAndBroadcast(int snapshotId, ActivityStatus activityStatus);
        int SnapshotId { get; }
        string ToString();
    }
}
