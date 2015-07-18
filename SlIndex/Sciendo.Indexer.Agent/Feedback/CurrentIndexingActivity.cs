using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Agent.Feedback
{
    public class CurrentIndexingActivity
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

        public void SetAndBroadcast(ActivityStatus activityStatus)
        {
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
            ActivityStatus = ActivityStatus.None;
            Details = string.Empty;
            BroadcastCurrentActivity();
            BroadcastDetails();
        }
        public ActivityStatus ActivityStatus { get; private set; }

        public string Details { get; private set; }

        public override string ToString()
        {
            return string.Format("Analysis for Snapshot with Id {0} change status to: {1}", ActivityStatus.ToString());
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
