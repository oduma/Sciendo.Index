using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Contracts.Analysis
{
    [ServiceContract(Namespace = "http://Sciendo.Music.Analysis")]
    public interface IAnalysis
    {
        [OperationContract]
        Snapshot[] GetAllAnalysisSnaphots();

        [OperationContract]
        Element[] GetAnalysis(string fromPath, int snapshotId);

        [OperationContract]
        Snapshot CreateNewSnapshot(string name);

        [OperationContract]
        int CreateElements(Element[] newElements);

        [OperationContract]
        StatisticRow[] GetStatistics(string fromPath, int snapshotId);

    }
}
