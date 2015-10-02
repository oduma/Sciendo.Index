using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Music.Real.Feedback
{
    public class CurrentStatisticsActivity : Sciendo.Music.Real.Feedback.ICurrentStatisticsActivity
    {

        private readonly static Lazy<CurrentStatisticsActivity> _instance = new Lazy<CurrentStatisticsActivity>(() => new CurrentStatisticsActivity(GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>().Clients));
        
        private CurrentStatisticsActivity(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            SnapshotId = 0;
            ActivityStatus = ActivityStatus.None;
        }

        public static CurrentStatisticsActivity Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void SetAndBroadcast(int snapshotId, ActivityStatus activityStatus)
        {
            SnapshotId = snapshotId;
            ActivityStatus = activityStatus;
            BroadcastCurrentActivity();

        }

        public void BroadcastDetails(string details)
        {
            Details = details;
            BroadcastDetails();
        }
        public void ClearAndBroadcast()
        {
            SnapshotId = 0;
            ActivityStatus = ActivityStatus.None;
            Details = string.Empty;
            BroadcastCurrentActivity();
            BroadcastDetails();
        }
        public int SnapshotId { get; private set; }

        public ActivityStatus ActivityStatus { get; private set; }

        public string Details { get; private set; }

        public override string ToString()
        {
            if (SnapshotId <= 0)
                return string.Empty;
            return string.Format("Analysis for Snapshot with Id {0} change status to: {1}", SnapshotId, ActivityStatus.ToString());
        }

        private void BroadcastDetails()
        {
            Clients.All.updateAnalysisDetails(Details);
        }

        private void BroadcastCurrentActivity()
        {
            Clients.All.updateCurrentAnalysisActivity(this.ToString());
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

    }
}
