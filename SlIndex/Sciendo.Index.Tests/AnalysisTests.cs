using NUnit.Framework;
using Sciendo.Music.Agent.Service;
using Sciendo.Music.Contracts.Analysis;
using Sciendo.Music.Real.Analysis;
using Sciendo.Music.Solr.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            AnalysisService analysisService = new AnalysisService("","","",null);
            var actual = analysisService.GetAllAnalysisSnaphots();
            Assert.IsNotNull(actual);
            Assert.Greater(actual.Length,1);
        }
        [Test]
        public void GetElementsBySnapshotOk()
        {
            AnalysisService analysisService = new AnalysisService("","","",null);
            var actual = analysisService.GetAnalysis("leaf", 3);
            Assert.IsNotNull(actual);
            Assert.Greater(actual.Length, 1);
        }

        [Test]
        public void CreateSnapshotOk()
        {
            using(TransactionScope transactionScope= new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                AnalysisService analysisService = new AnalysisService("","","",null);
                var actual = analysisService.CreateNewSnapshot("test");
                Assert.IsNotNull(actual);
                Assert.Greater(actual.SnapshotId, 1);
                analysisService = new AnalysisService("","","",null);
                var actualsnapshots = analysisService.GetAllAnalysisSnaphots();
                Assert.AreEqual(4, actualsnapshots.Length);
            }
        }
        [Test]
        public void CreateElementsOk()
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                AnalysisService analysisService = new AnalysisService("","","",null);
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

        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest1()
        {
            Assert.AreEqual(IndexedFlag.NotIndexed, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetExceptionMock("something.else")));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest2()
        {
            Assert.AreEqual(IndexedFlag.NotIndexed, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetNullResultsMock("something.else")));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest3()
        {
            Assert.AreEqual(IndexedFlag.NotIndexed, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetNullResultRowsMock("something.else")));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest4()
        {
            Assert.AreEqual(IndexedFlag.NotIndexed, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetNoResultRowsMock("something.else")));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagNotIndexedTest5()
        {
            Assert.AreEqual(IndexedFlag.NotIndexed, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetOneEmptyResultRowsMock("something.else")));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingButTheAlbumTest()
        {
            Assert.AreEqual(IndexedFlag.Indexed|IndexedFlag.IndexedArtist|IndexedFlag.IndexedLyrics|IndexedFlag.IndexedTitle, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetIndexedResultMock("something.else",MissingType.Album)));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingButTheArtistTest()
        {
            Assert.AreEqual(IndexedFlag.Indexed | IndexedFlag.IndexedAlbum | IndexedFlag.IndexedLyrics | IndexedFlag.IndexedTitle, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.Artist)));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingButTheTitleTest()
        {
            Assert.AreEqual(IndexedFlag.Indexed | IndexedFlag.IndexedArtist | IndexedFlag.IndexedLyrics | IndexedFlag.IndexedAlbum, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.Title)));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingButTheLyricsTest()
        {
            Assert.AreEqual(IndexedFlag.Indexed | IndexedFlag.IndexedArtist | IndexedFlag.IndexedAlbum | IndexedFlag.IndexedTitle, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.Lyrics)));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedOnlyFileTest()
        {
            Assert.AreEqual(IndexedFlag.Indexed , Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.All)));
        }
        [Test]
        public void AnalysisUtilsIndexedFlagIndexedEverythingTest()
        {
            Assert.AreEqual(IndexedFlag.Indexed | IndexedFlag.IndexedArtist | IndexedFlag.IndexedAlbum | IndexedFlag.IndexedTitle |IndexedFlag.IndexedLyrics, Utils.GetIndexedFlag("something.else", SolrResultsProviderMocks.GetIndexedResultMock("something.else", MissingType.None)));
        }

        [Test]
        //[Ignore("performance test")]
        public void AnalyseThisTest()
        {
            var globalStopwatch = new Stopwatch();
            globalStopwatch.Start();
            IResultsProvider resultsProvider = new SolrResultsProvider();
            AnalysisService svc = new AnalysisService(@"c:\code\m\music", @"c:\code\m\lyrics", "*.mp3|*.ogg", resultsProvider);

            Stopwatch stopwatch = new Stopwatch();

            var snapshot = svc.CreateNewSnapshot("new test");
            stopwatch.Start();

            var processed = svc.AnaliseThis("c:\\code\\m\\music\\b\\beck\\Clock", snapshot.SnapshotId);

            stopwatch.Stop();
            globalStopwatch.Stop();
            TimeSpan timeSpan = new TimeSpan(stopwatch.ElapsedTicks);
            TimeSpan globalTimeSpan = new TimeSpan(globalStopwatch.ElapsedTicks);

            Console.WriteLine("Processed {0} in {1}:{2}.{3}", processed, timeSpan.Minutes, timeSpan.Seconds,timeSpan.Milliseconds);
            Console.WriteLine("Global time {0}:{1}.{2}", globalTimeSpan.Minutes, globalTimeSpan.Seconds, globalTimeSpan.Milliseconds);
        }
    }
}
