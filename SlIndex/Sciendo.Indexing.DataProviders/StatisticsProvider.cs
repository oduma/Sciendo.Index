using Sciendo.Music.Contracts.Analysis;
using Sciendo.Music.DataProviders.Models.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.DataProviders
{
    public class StatisticsProvider:IStatisticsProvider
    {
        IAnalysis _svc = new AnalysisClient();

        public StatisticsModel GetStatisticsModel()
        {
            return new StatisticsModel { Snapshots = _svc.GetAllAnalysisSnaphots() };
        }


        public StatisticRow[] GetSnapshotDetails(string fromPath, int snapshotId)
        {
            return _svc.GetStatistics(fromPath, snapshotId);
        }


        public Snapshot TakeNewSnapshot(string name)
        {
            var snapshot = _svc.CreateNewSnapshot(name);
            _svc.AnaliseThis(string.Empty, snapshot.SnapshotId);
            return snapshot;
        }
    }
}
