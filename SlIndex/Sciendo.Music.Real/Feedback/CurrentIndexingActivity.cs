using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Real.Feedback
{
    public class CurrentIndexingActivity : ICurrentFileActivity
    {
        private readonly static Lazy<CurrentIndexingActivity> _instance 
            = new Lazy<CurrentIndexingActivity>(() => new CurrentIndexingActivity(GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>().Clients));
        
        private CurrentIndexingActivity(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            ActivityStatus = ActivityStatus.None;
        }

        public static CurrentIndexingActivity Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void SetAndBroadcast(string fromPath, ActivityStatus activityStatus)
        {
            ActivityStatus = activityStatus;
            FromPath = fromPath;
            BroadcastCurrentActivity();

        }

        public void BroadcastDetails(string details)
        {
            Details = details;
            BroadcastDetails();
        }
        public void ClearAndBroadcast()
        {
            ActivityStatus = ActivityStatus.None;
            Details = string.Empty;
            BroadcastCurrentActivity();
            BroadcastDetails();
        }
        public ActivityStatus ActivityStatus { get; private set; }

        public string Details { get; private set; }

        public override string ToString()
        {
            if (ActivityStatus == ActivityStatus.None)
                return "";
            return string.Format("Indexing from: {0} change status to: {1}",FromPath, ActivityStatus.ToString());
        }

        private void BroadcastDetails()
        {
            Clients.All.updateIndexingDetails(Details);
        }

        private void BroadcastCurrentActivity()
        {
            Clients.All.updateCurrentIndexingActivity(this.ToString());
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        public string FromPath { get; private set; }
    }
}
