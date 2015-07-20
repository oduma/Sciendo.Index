using Microsoft.AspNet.SignalR;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Real.Feedback
{
    public class FeedbackHub:Hub
    {
        private CurrentStatisticsActivity _currentStatisticsActivity;
        private CurrentIndexingActivity _currentIndexingActivity;

        public FeedbackHub (CurrentStatisticsActivity currentStatisticsActivity, CurrentIndexingActivity currentIndexingActivity)
        {
            _currentStatisticsActivity = currentStatisticsActivity;
            _currentIndexingActivity = currentIndexingActivity;
        }

        public FeedbackHub()
        {
            _currentStatisticsActivity = CurrentStatisticsActivity.Instance;
            _currentIndexingActivity = CurrentIndexingActivity.Instance;
        }

        public string GetCurrentAnalysisStatus()
        {
            return _currentStatisticsActivity.ToString();
        }

        public string GetCurrentIndexingStatus()
        {
            return _currentIndexingActivity.ToString();
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
