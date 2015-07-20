using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Real.Feedback
{
    public class CurrentGetLyricsActivity:ICurrentFileActivity
    {
        private readonly static Lazy<CurrentGetLyricsActivity> _instance
            = new Lazy<CurrentGetLyricsActivity>(() => new CurrentGetLyricsActivity(GlobalHost.ConnectionManager.GetHubContext<FeedbackHub>().Clients));

        private CurrentGetLyricsActivity(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            ActivityStatus = ActivityStatus.None;
        }

        public static CurrentGetLyricsActivity Instance
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
            return string.Format("AcquireLyrics from: {0} change status to: {1}", FromPath, ActivityStatus.ToString());
        }

        private void BroadcastDetails()
        {
            Clients.All.updateGetLyricsDetails(Details);
        }

        private void BroadcastCurrentActivity()
        {
            Clients.All.updateCurrentGetLyricsActivity(this.ToString());
        }

        private IHubConnectionContext<dynamic> Clients { get; set; }

        public string FromPath { get; private set; }
    }
}
