using Sciendo.Music.Contracts.Analysis;
using Sciendo.Music.DataProviders.Models.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.DataProviders
{
    public interface IStatisticsProvider
    {
        StatisticsModel GetStatisticsModel();

        StatisticRow[] GetSnapshotDetails(string fromPath, int snapshotId);

        Snapshot TakeNewSnapshot(string name);
    }
}
