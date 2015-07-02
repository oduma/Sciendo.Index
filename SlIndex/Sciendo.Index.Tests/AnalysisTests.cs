using NUnit.Framework;
using Sciendo.Music.Agent.Service;
using Sciendo.Music.Contracts.Analysis;
using Sciendo.Music.Real.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Sciendo.Music.Tests
{
    [TestFixture]
    public class AnalysisTests
    {
        [Test]
        public void GetAllSnapshotsOk()
        {
            AnalysisService analysisService = new AnalysisService("","","");
            var actual = analysisService.GetAllAnalysisSnaphots();
            Assert.IsNotNull(actual);
            Assert.Greater(actual.Length,1);
        }
        [Test]
        public void GetElementsBySnapshotOk()
        {
            AnalysisService analysisService = new AnalysisService("","","");
            var actual = analysisService.GetAnalysis("leaf", 3);
            Assert.IsNotNull(actual);
            Assert.Greater(actual.Length, 1);
        }

        [Test]
        public void CreateSnapshotOk()
        {
            using(TransactionScope transactionScope= new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                AnalysisService analysisService = new AnalysisService("","","");
                var actual = analysisService.CreateNewSnapshot("test");
                Assert.IsNotNull(actual);
                Assert.Greater(actual.SnapshotId, 1);
                analysisService = new AnalysisService("","","");
                var actualsnapshots = analysisService.GetAllAnalysisSnaphots();
                Assert.AreEqual(4, actualsnapshots.Length);
            }
        }
        [Test]
        public void CreateElementsOk()
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                AnalysisService analysisService = new AnalysisService("","","");
                var actual = analysisService.CreateElements(new Element[] { new Element {IndexedFlag = IndexedFlag.NotIndexed, LyricsFileFlag = LyricsFileFlag.HasLyricsFile | LyricsFileFlag.LyricsFileOk, MusicFileFlag = MusicFileFlag.HasTag | MusicFileFlag.IsMusicFile | MusicFileFlag.HasArtistTag, Name = "test element one", SnapshotId = 3 } });
                Assert.IsNotNull(actual);
                Assert.AreEqual(1, actual);
            }

        }

        [Test]
        public void AnalysisUtilsGetMusciFileFlagNoMusicFileTest()
        {
            Assert.AreEqual(MusicFileFlag.NotAMusicFile, Utils.GetMusicFlag("something.else", "*.mp3|*.ogg",null));
        }
        [Test]
        public void AnalysisUtilsGetMusciFileFlagMusicFileNoTagTest()
        {
            Assert.AreEqual(MusicFileFlag.IsMusicFile, Utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg",Mp3StreamMockUtils.MockedMp3FileLoader_NoTag));
        }
        [Test]
        public void AnalysisUtilsGetMusciFileFlagMusicFileUnknownTagTest()
        {
            Assert.AreEqual(MusicFileFlag.IsMusicFile, Utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_UnknownTag));
        }
        [Test]
        public void AnalysisUtilsGetMusciFileFlagMusicFileNoTagTest1()
        {
            Assert.AreEqual(MusicFileFlag.IsMusicFile, Utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_NoTag1));
        }
        [Test]
        public void AnalysisUtilsGetMusciFileFlagMusicFileErrorTagTest()
        {
            Assert.AreEqual(MusicFileFlag.IsMusicFile, Utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_ErrorTag));
        }
        [Test]
        public void AnalysisUtilsGetMusciFileFlagMusicFileEmptyTagTest()
        {
            Assert.AreEqual(MusicFileFlag.IsMusicFile, Utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_EmptyTag));
        }
        [Test]
        public void AnalysisUtilsGetMusciFileFlagMusicFileNoArtistTest()
        {
            Assert.AreEqual(MusicFileFlag.IsMusicFile|MusicFileFlag.HasAlbumTag|MusicFileFlag.HasTitleTag|MusicFileFlag.HasTag, Utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_EverythingButArtist));
        }
        [Test]
        public void AnalysisUtilsGetMusciFileFlagMusicFileNoAlbumTest()
        {
            Assert.AreEqual(MusicFileFlag.IsMusicFile | MusicFileFlag.HasArtistTag | MusicFileFlag.HasTitleTag | MusicFileFlag.HasTag, Utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_TagOk));
        }
        [Test]
        public void AnalysisUtilsGetMusciFileFlagMusicFileNoTitleTest()
        {
            Assert.AreEqual(MusicFileFlag.IsMusicFile | MusicFileFlag.HasArtistTag | MusicFileFlag.HasAlbumTag | MusicFileFlag.HasTag, Utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_EverythingButTitle));
        }
        [Test]
        public void AnalysisUtilsGetMusciFileFlagMusicFileFullTagTest()
        {
            Assert.AreEqual(MusicFileFlag.IsMusicFile | MusicFileFlag.HasArtistTag | MusicFileFlag.HasAlbumTag | MusicFileFlag.HasTag | MusicFileFlag.HasTitleTag, Utils.GetMusicFlag("something.mp3", "*.mp3|*.ogg", Mp3StreamMockUtils.MockedMp3FileLoader_FullTag));
        }

        [Test]
        public void AnalysisUtilsLyricsFileFlagNoFileTest1()
        {
            Assert.AreEqual(LyricsFileFlag.NoLyricsFile, Utils.GetLyricsFlag("something.else", "*.mp3|*.ogg", "", ""));
        }
        [Test]
        public void AnalysisUtilsLyricsFileFlagNoFileTest2()
        {
            Assert.AreEqual(LyricsFileFlag.NoLyricsFile, Utils.GetLyricsFlag(@"TestData\Music\Sub1\MockOggWithoutLyrics.ogg", "*.mp3|*.ogg", @"TestData\Music", @"TestData\Lyrics"));
        }
        [Test]
        public void AnalysisUtilsLyricsFileFlagLyricsNotFoundTest()
        {
            Assert.AreEqual(LyricsFileFlag.HasLyricsFile | LyricsFileFlag.LyricsFileNoLyrics, Utils.GetLyricsFlag(@"TestData\Music\Sub1\MockOggLyricsNotFound.ogg", "*.mp3|*.ogg", @"TestData\Music", @"TestData\Lyrics"));
        }
        [Test]
        public void AnalysisUtilsLyricsFileFlagLyricsWithErrorTest()
        {
            Assert.AreEqual(LyricsFileFlag.HasLyricsFile | LyricsFileFlag.LyricsFileWithError, Utils.GetLyricsFlag(@"TestData\Music\Sub1\MockOggWithErrorLyrics.ogg", "*.mp3|*.ogg", @"TestData\Music", @"TestData\Lyrics"));
        }
        [Test]
        public void AnalysisUtilsLyricsFileFlagOkTest()
        {
            Assert.AreEqual(LyricsFileFlag.HasLyricsFile | LyricsFileFlag.LyricsFileOk, Utils.GetLyricsFlag(@"TestData\Music\Sub1\MockOggWithLyrics.ogg", "*.mp3|*.ogg", @"TestData\Music", @"TestData\Lyrics"));
        }
    }
}
