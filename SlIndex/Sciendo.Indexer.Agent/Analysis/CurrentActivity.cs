using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sciendo.Music.Agent.Analysis
{
    public class CurrentActivity
    {

        private readonly static Lazy<CurrentActivity> _instance = new Lazy<CurrentActivity>(() => new CurrentActivity(GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>().Clients));
        
        private CurrentActivity(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            SnapshotId = 0;
            ActivityStatus = Analysis.ActivityStatus.None;
        }

        public static CurrentActivity Instance
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
            Clients.All.updateDetails(Details);
        }

        private void BroadcastCurrentActivity()
        {
            Clients.All.updateCurrentActivity(this.ToString());
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

    }
}
