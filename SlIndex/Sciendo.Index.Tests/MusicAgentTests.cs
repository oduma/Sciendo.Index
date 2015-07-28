using System.Linq;
using NUnit.Framework;
using Sciendo.Music.Agent;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Real.Procesors.MusicSourced;

namespace Sciendo.Music.Tests
{
    [TestFixture]
    public class MusicAgentTests
    {
        [Test]
        public void MusicAgentConstructedOk()
        {
            MusicAgent musicAgent = new MusicAgent();
            Assert.AreEqual(2,IOC.Container.GetInstance().ResolveAll<IndexingFilesProcessor>().Count());
            Assert.AreEqual(2, IOC.Container.GetInstance().ResolveAll<MusicToLyricsFilesProcessor>().Count());
            Assert.AreEqual(2, IOC.Container.GetInstance().ResolveAll<IFolderMonitor>().Count());
            Assert.IsNotNull(musicAgent.MonitoringInstance);

        }
    }
}
