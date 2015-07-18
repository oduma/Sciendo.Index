using Microsoft.AspNet.SignalR;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Agent.Feedback
{
    public class FeedbackHub:Hub
    {
        private CurrentStatisticsActivity _currentActivity;
        public FeedbackHub (CurrentStatisticsActivity currentActivity)
        {
            _currentActivity = currentActivity;
        }

        public FeedbackHub()
        {
            _currentActivity = CurrentStatisticsActivity.Instance;
        }
        public string GetCurrentStatus()
        {
            return _currentActivity.ToString();
        }

        //public void ChangeStatus(string message)
        //{
        //    Clients.All.sendMessage("change status: " + message);
        //}
        //public void Publish(string message)
        //{
        //    Clients.All.sendMessage("publish: " + message);
        //}

    }
}
