using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Sciendo.Music.Agent;
using Sciendo.Music.Contracts.Monitoring;
using Sciendo.Music.Mocks.Processing;
using Sciendo.Music.Real.Procesors.LyricsSourced;
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
            Assert.AreEqual(2,IOC.Container.GetInstance().ResolveAll<MusicFilesProcessor>().Count());
            Assert.AreEqual(2, IOC.Container.GetInstance().ResolveAll<LyricsFilesProcessor>().Count());
            Assert.AreEqual(2, IOC.Container.GetInstance().ResolveAll<MusicToLyricsFilesProcessor>().Count());
            Assert.AreEqual(4, IOC.Container.GetInstance().ResolveAll<IFolderMonitor>().Count());
            Assert.IsNotNull(musicAgent.MonitoringInstances);
            Assert.AreEqual(2,musicAgent.MonitoringInstances.Count());
            Assert.Contains(MonitoringType.Lyrics,musicAgent.MonitoringInstances.Keys);
            Assert.Contains(MonitoringType.Music, musicAgent.MonitoringInstances.Keys);

        }

        [Test]
        public void ResolveIOCComponentsToMock()
        {
            MusicAgent musicAgent = new MusicAgent();
            musicAgent.ResolveComponents("mock","mock",20);
            var currentWorkigSet = musicAgent.AgentService.GetCurrentWorkingSet();
            Assert.AreEqual(typeof(MockMusicFilesProcessor),currentWorkigSet.MusicFilesProcessorType);
            Assert.AreEqual(typeof(MockMusicToLyricsFilesProcessor), currentWorkigSet.MusicToLyricsFilesProcessorType);
            Assert.AreEqual(typeof(MockLyricsFilesProcessor), currentWorkigSet.LyricsFilesProcessorType);
        }
    }
}
