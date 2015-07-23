using System.Linq;
using NUnit.Framework;
using Sciendo.Music.Agent;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Mocks.Processing;
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

        [Test]
        public void ResolveIOCComponentsToMock()
        {
            MusicAgent musicAgent = new MusicAgent();
            musicAgent.ResolveComponents("mock");
            var currentWorkigSet = musicAgent.AgentService.GetCurrentWorkingSet();
            Assert.AreEqual(typeof(MockIndexingFilesProcessor),currentWorkigSet.IndexingFilesProcessorType);
            Assert.AreEqual(typeof(MockMusicToLyricsFilesProcessor), currentWorkigSet.LyricsAcquirerFilesProcessorType);
        }
    }
}
