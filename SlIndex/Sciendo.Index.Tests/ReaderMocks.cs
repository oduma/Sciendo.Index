using Rhino.Mocks;
using Sciendo.Music.Agent.Service;
using Sciendo.Music.Contracts.Analysis;
using Sciendo.Music.Real.Analysis;
using Sciendo.Music.Real.Feedback;
using Sciendo.Music.Solr.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Tests
{
    public static class ReaderMocks
    {
        public static MusicFileFlag somethingHere(string file, string pattern)
        {
            return MusicFileFlag.IsMusicFile;
        }
        public static ICurrentFileActivity MockFileActivityInvalidPath(string filePath,Action<string,ActivityStatus> recordActivity)
        {
            var mockFileActivity = MockRepository.GenerateStrictMock<ICurrentFileActivity>();
            mockFileActivity.Stub(m => m.SetAndBroadcast(filePath, ActivityStatus.InProgress)).Do(recordActivity);
            mockFileActivity.Stub(m => m.SetAndBroadcast(filePath, ActivityStatus.Stopped)).Do(recordActivity);
            mockFileActivity.Expect(m => m.SetAndBroadcast(filePath, ActivityStatus.InProgress));
            mockFileActivity.Expect(m => m.SetAndBroadcast(filePath, ActivityStatus.Stopped));
            
            
            return mockFileActivity;
        }

        public static ICurrentFileActivity MockFileActivityDelete(string filePath, Action<string, ActivityStatus> recordActivity)
        {
            var mockFileActivity = MockRepository.GenerateStrictMock<ICurrentFileActivity>();
            mockFileActivity.Stub(m => m.SetAndBroadcast(filePath, ActivityStatus.InProgress)).Do(recordActivity);
            mockFileActivity.Stub(m => m.BroadcastDetails("Deleting: " + filePath));
            mockFileActivity.Stub(m => m.SetAndBroadcast(filePath, ActivityStatus.Stopped)).Do(recordActivity);
            mockFileActivity.Expect(m => m.SetAndBroadcast(filePath, ActivityStatus.InProgress));
            mockFileActivity.Expect(m => m.BroadcastDetails("Deleting: " + filePath));
            mockFileActivity.Expect(m => m.SetAndBroadcast(filePath, ActivityStatus.Stopped));


            return mockFileActivity;
        }

        internal static ICurrentFileActivity MockFileActivityProcessFolder(string folder, Action<string, ActivityStatus> recordActivity)
        {
            var mockFileActivity = MockRepository.GenerateStrictMock<ICurrentFileActivity>();
            mockFileActivity.Stub(m => m.SetAndBroadcast(folder, ActivityStatus.InProgress)).Do(recordActivity);
            mockFileActivity.Stub(m => m.SetAndBroadcast(folder, ActivityStatus.Stopped)).Do(recordActivity);
            mockFileActivity.Expect(m => m.SetAndBroadcast(folder, ActivityStatus.InProgress));
            foreach (var s in Directory.GetDirectories(folder, "*", SearchOption.AllDirectories))
                mockFileActivity.Expect(m => m.BroadcastDetails("Processing: " + s));

            mockFileActivity.Expect(m => m.BroadcastDetails("Processing: " + folder));


            return mockFileActivity;
        }

        internal static ICurrentFileActivity MockFileActivityProcessFile(string filePath, Action<string, ActivityStatus> recordActivity)
        {
            var mockFileActivity = MockRepository.GenerateStrictMock<ICurrentFileActivity>();
            mockFileActivity.Stub(m => m.SetAndBroadcast(filePath, ActivityStatus.InProgress)).Do(recordActivity);
            mockFileActivity.Stub(m => m.SetAndBroadcast(filePath, ActivityStatus.Stopped)).Do(recordActivity);
            mockFileActivity.Expect(m => m.SetAndBroadcast(filePath, ActivityStatus.InProgress));
            mockFileActivity.Expect(m => m.BroadcastDetails("Processing: " + filePath));


            return mockFileActivity;
        }
                    

        internal static ICurrentStatisticsActivity MockStatisticsFromRootFolder(int snapshotId, string rootFolder, Action<int,ActivityStatus> recordActivity)
        {
            var mockStatisticsActivity = MockRepository.GenerateStrictMock<ICurrentStatisticsActivity>();
            mockStatisticsActivity.Stub(m => m.SetAndBroadcast(snapshotId, ActivityStatus.Starting)).Do(recordActivity);
            mockStatisticsActivity.Stub(m => m.SetAndBroadcast(snapshotId, ActivityStatus.InProgress)).Do(recordActivity);
            foreach(var folder in Directory.GetDirectories(rootFolder,"*",SearchOption.AllDirectories))
            {
                mockStatisticsActivity.Expect(m => m.BroadcastDetails(folder));
            }
            mockStatisticsActivity.Expect(m => m.BroadcastDetails(rootFolder));
            mockStatisticsActivity.Expect(m => m.BroadcastDetails("Total files analysed 1"));

            mockStatisticsActivity.Stub(m => m.SetAndBroadcast(snapshotId, ActivityStatus.Stopped)).Do(recordActivity);
            mockStatisticsActivity.Expect(m => m.ClearAndBroadcast());

            return mockStatisticsActivity;
        }

        internal static IAnalysis MockAnalysisServiceForAnalyseThis(string rootMusic, string rootLyrics,string pattern, int snapshotId, Action<int,ActivityStatus> recordActivity)
        {
            var mockAnalysis = MockRepository.GenerateStrictMock<AnalysisService>(rootMusic, 
                rootLyrics, 
                pattern, 
                MockStatisticsFromRootFolder(snapshotId, rootMusic, recordActivity),
                MockUtilsForTestFolder(rootMusic,pattern,rootMusic,rootLyrics,somethingHere));
           
            mockAnalysis.Stub(m => m.CreateElements(new Element[]{})).Return(0);
            mockAnalysis.Stub(m => m.CreateElements(new Element[]{})).Return(1);


            
            return mockAnalysis;

        }

        internal static Utils MockUtilsForTestFolder(string fromFolder, 
            string pattern,string musicRootFolder,string lyricsRootFolder,Func<string,string,MusicFileFlag> something)
        {
            var mockFlagsUtils = MockRepository.GenerateMock<Utils>(new SolrResultsProvider());

            foreach(var file in Directory.GetFiles(fromFolder,"*",SearchOption.AllDirectories))
            {
                switch (Path.GetExtension(file))
                {
                    case ".mp3":
                        {
                            mockFlagsUtils.Stub(m => m.GetIndexedFlag(file)).Return(IndexedFlag.Indexed & IndexedFlag.IndexedAlbum & IndexedFlag.IndexedArtist & IndexedFlag.IndexedLyrics & IndexedFlag.IndexedTitle);
                            mockFlagsUtils.Stub(m => m.GetLyricsFlag(file, pattern, musicRootFolder, lyricsRootFolder)).Return(LyricsFileFlag.HasLyricsFile & LyricsFileFlag.LyricsFileOk);
                            mockFlagsUtils.Stub(m => m.GetMusicFlag(file, pattern)).Do(something).Return(MusicFileFlag.IsMusicFile & MusicFileFlag.HasAlbumTag & MusicFileFlag.HasArtistTag & MusicFileFlag.HasTitleTag);
                            break;
                        }
                    case ".ogg":
                        {
                            mockFlagsUtils.Stub(m => m.GetIndexedFlag(file)).Return(IndexedFlag.Indexed & IndexedFlag.IndexedAlbum & IndexedFlag.IndexedArtist & IndexedFlag.IndexedTitle);
                            mockFlagsUtils.Stub(m => m.GetLyricsFlag(file, pattern, musicRootFolder, lyricsRootFolder)).Return(LyricsFileFlag.HasLyricsFile & LyricsFileFlag.LyricsFileWithError);
                            mockFlagsUtils.Stub(m => m.GetMusicFlag(file, pattern)).Return(MusicFileFlag.IsMusicFile & MusicFileFlag.HasArtistTag & MusicFileFlag.HasTitleTag);
                            break;
                        }
                    default:
                        {
                            mockFlagsUtils.Stub(m => m.GetIndexedFlag(file)).Return(IndexedFlag.NotIndexed);
                            mockFlagsUtils.Stub(m => m.GetLyricsFlag(file, pattern, musicRootFolder, lyricsRootFolder)).Return(LyricsFileFlag.NoLyricsFile);
                            mockFlagsUtils.Stub(m => m.GetMusicFlag(file, pattern)).Return(MusicFileFlag.NotAMusicFile);
                            break;
                        }
                }
            }
            return mockFlagsUtils;
        }
    }
}
