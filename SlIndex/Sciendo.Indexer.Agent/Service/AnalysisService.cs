using Microsoft.AspNet.SignalR.Hubs;
using Sciendo.Common.Logging;
using Sciendo.Music.Real.Feedback;
using Sciendo.Music.Contracts.Analysis;
using Sciendo.Music.Data;
using Sciendo.Music.Real.Analysis;
using Sciendo.Music.Solr.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sciendo.Music.Agent.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class AnalysisService:IAnalysis
    {
        private readonly string _musicSourceFolder;
        private readonly string _lyricsSourceFolder; 
        private readonly string _pattern;
        private readonly IResultsProvider _resultsProvider;

        public AnalysisService(string musicSourceFolder, string lyricsSourceFolder, string pattern, IResultsProvider solrResultsProvider)
        {
            LoggingManager.Debug("Constructing Analysis Service...");
            _musicSourceFolder = musicSourceFolder;
            _lyricsSourceFolder = lyricsSourceFolder;
            _pattern = pattern;
            _resultsProvider = solrResultsProvider;
            LoggingManager.Debug("Analysis service constructed with: " + _musicSourceFolder);
        }

        public Snapshot[] GetAllAnalysisSnaphots()
        {
            using (Statistics container = new Statistics())
            {
                container.Database.Log = LoggingManager.Debug;
                return container.Snapshots.ToArray();
            }
        }


        public Element[] GetAnalysis(string fromPath, int snapshotId)
        {
            using (Statistics container = new Statistics())
            {
                return container.Elements.Where(e => e.SnapshotId == snapshotId && e.Name.StartsWith(fromPath)).ToArray();
            }
        }


        public Snapshot CreateNewSnapshot(string name)
        {
            using(Statistics container = new Statistics())
            {
                container.Database.Log = LoggingManager.Debug;
                var newSnapshot = new Snapshot { Name = name };
                container.Snapshots.Add(newSnapshot);
                container.SaveChanges();
                return newSnapshot;
            }
        }


        public int CreateElements(Element[] newElements)
        {
            try
            {
                using (Statistics container = new Statistics())
                {
                    container.Database.Log = LoggingManager.Debug;
                    container.Elements.AddRange(newElements);
                    container.SaveChanges();
                    return newElements.Length;
                }
            }
            catch(Exception ex)
            {
                LoggingManager.LogSciendoSystemError(ex);
                return 0;
            }
        }


        public StatisticRow[] GetStatistics(string fromPath, int snapshotId)
        {

            if(string.IsNullOrEmpty(fromPath))
            {
                //GetTotals for the source folder
                using (Statistics container = new Statistics())
                {
                    container.Database.Log = LoggingManager.Debug;
                    return new StatisticRow[] {GetAggregatedStatisticRow(container.Elements.Where(e=>e.SnapshotId==snapshotId),_musicSourceFolder)};
                }
            }
            else
            {
                using(Statistics container = new Statistics())
                {
                    container.Database.Log = LoggingManager.Debug;
                    List<StatisticRow> statisticRows = new List<StatisticRow>();

                    var subElements = container.Elements.Where(e => e.SnapshotId == snapshotId
                        && e.Name.StartsWith(fromPath)).Select(e => e.Name).ToList();
                    foreach(var folder in subElements.Select(s=>fromPath+@"\" + s.Replace(fromPath+@"\","").Split(new char[]{'\\'})[0]).Distinct())
                    {

                        statisticRows.Add(GetAggregatedStatisticRow(container.Elements.Where(e=>e.SnapshotId==snapshotId && e.Name.StartsWith(folder)),folder));
                    }
                    statisticRows.AddRange(GetStatisticRows(container.Elements.Where(s=>s.SnapshotId==snapshotId && (s.Name.Replace(fromPath+@"\","").IndexOf(@"\")<=0))));
                    return statisticRows.ToArray();
                }
            }
        }

        private IEnumerable<StatisticRow> GetStatisticRows(IEnumerable<Element> elements)
        {
            return elements.Select(e => new StatisticRow
            {
                Files = 1,
                MusicFiles = ((e.MusicFileFlag & MusicFileFlag.IsMusicFile) == MusicFileFlag.IsMusicFile) ? 1 : 0,
                Tagged = ((e.MusicFileFlag & MusicFileFlag.HasTag) == MusicFileFlag.HasTag) ? 1 : 0,
                Name = e.Name,
                NotTagged = ((e.MusicFileFlag & MusicFileFlag.HasTag) == MusicFileFlag.HasTag) ? 0 : 1,
                AlbumTag = ((e.MusicFileFlag & MusicFileFlag.HasAlbumTag) == MusicFileFlag.HasAlbumTag) ? 1 : 0,
                ArtistTag = ((e.MusicFileFlag & MusicFileFlag.HasArtistTag) == MusicFileFlag.HasArtistTag) ? 1 : 0,
                TitleTag = ((e.MusicFileFlag & MusicFileFlag.HasTitleTag) == MusicFileFlag.HasTitleTag) ? 1 : 0,
                Lyrics = ((e.LyricsFileFlag & LyricsFileFlag.HasLyricsFile) == LyricsFileFlag.HasLyricsFile) ? 1 : 0,
                LyricsOk = ((e.LyricsFileFlag & LyricsFileFlag.LyricsFileOk) == LyricsFileFlag.LyricsFileOk) ? 1 : 0,
                LyricsNotFound = ((e.LyricsFileFlag & LyricsFileFlag.LyricsFileNoLyrics) == LyricsFileFlag.LyricsFileNoLyrics) ? 1 : 0,
                LyricsError = ((e.LyricsFileFlag & LyricsFileFlag.LyricsFileWithError) == LyricsFileFlag.LyricsFileWithError) ? 1 : 0,
                NotIndexed = (e.IndexedFlag==IndexedFlag.NotIndexed) ? 1 : 0,
                Indexed = ((e.IndexedFlag & IndexedFlag.Indexed)==IndexedFlag.Indexed) ? 1 : 0,
                IndexedArtist = ((e.IndexedFlag & IndexedFlag.IndexedArtist) == IndexedFlag.IndexedArtist) ? 1 : 0,
                IndexedAlbum = ((e.IndexedFlag & IndexedFlag.IndexedAlbum) == IndexedFlag.IndexedAlbum) ? 1 : 0,
                IndexedTitle = ((e.IndexedFlag & IndexedFlag.IndexedTitle) == IndexedFlag.IndexedTitle) ? 1 : 0,
                IndexedLyrics = ((e.IndexedFlag & IndexedFlag.IndexedLyrics) == IndexedFlag.IndexedLyrics) ? 1 : 0
            });
        }

        private StatisticRow GetAggregatedStatisticRow(IEnumerable<Element> elements, string forPath)
        {
            var musicFiles = elements.Count(e => (e.MusicFileFlag & MusicFileFlag.IsMusicFile) == MusicFileFlag.IsMusicFile);
            var tagged = elements.Count(e => (e.MusicFileFlag & MusicFileFlag.HasTag) == MusicFileFlag.HasTag);
            return new StatisticRow
            {
                Name = forPath,
                Files = elements.Count(),
                MusicFiles = musicFiles,
                Tagged = tagged,
                NotTagged = musicFiles - tagged,
                AlbumTag = elements.Count(e => (e.MusicFileFlag & MusicFileFlag.HasAlbumTag) == MusicFileFlag.HasAlbumTag),
                ArtistTag = elements.Count(e => (e.MusicFileFlag & MusicFileFlag.HasArtistTag) == MusicFileFlag.HasArtistTag),
                TitleTag = elements.Count(e => (e.MusicFileFlag & MusicFileFlag.HasTitleTag) == MusicFileFlag.HasTitleTag),
                Lyrics = elements.Count(e => (e.LyricsFileFlag & LyricsFileFlag.HasLyricsFile) == LyricsFileFlag.HasLyricsFile),
                LyricsOk = elements.Count(e => (e.LyricsFileFlag & LyricsFileFlag.LyricsFileOk) == LyricsFileFlag.LyricsFileOk),
                LyricsNotFound = elements.Count(e => (e.LyricsFileFlag & LyricsFileFlag.LyricsFileNoLyrics) == LyricsFileFlag.LyricsFileNoLyrics),
                LyricsError = elements.Count(e => (e.LyricsFileFlag & LyricsFileFlag.LyricsFileWithError) == LyricsFileFlag.LyricsFileWithError),
                NotIndexed = elements.Count(e => (e.IndexedFlag==IndexedFlag.NotIndexed)),
                Indexed = elements.Count(e => (e.IndexedFlag & IndexedFlag.Indexed) == IndexedFlag.Indexed),
                IndexedArtist = elements.Count(e => (e.IndexedFlag & IndexedFlag.IndexedArtist) == IndexedFlag.IndexedArtist),
                IndexedAlbum = elements.Count(e => (e.IndexedFlag & IndexedFlag.IndexedAlbum) == IndexedFlag.IndexedAlbum),
                IndexedTitle = elements.Count(e => (e.IndexedFlag & IndexedFlag.IndexedTitle) == IndexedFlag.IndexedTitle),
                IndexedLyrics = elements.Count(e => (e.IndexedFlag & IndexedFlag.IndexedLyrics) == IndexedFlag.IndexedLyrics)

            };
        }


        public void AnaliseThis(string folder, int snapshotId)
        {
            CurrentStatisticsActivity.Instance.SetAndBroadcast(snapshotId, ActivityStatus.Starting);
            if(string.IsNullOrEmpty(folder))
            {
                folder = _musicSourceFolder;
            }
            CurrentStatisticsActivity.Instance.SetAndBroadcast(snapshotId, ActivityStatus.InProgress);
            var elements = Directory.GetDirectories(folder,
                "*",
                SearchOption.AllDirectories).Where(d => Directory.GetFiles(d).Any()).AsParallel().Select(f => GetListOfElementsForFolder(f, snapshotId));

            var directElements = GetListOfElementsForFolder(folder, snapshotId);
            int retValue = 0;
            foreach(var newElements in elements)
            {
                retValue += CreateElements(newElements.ToArray());
            }
            retValue += CreateElements(directElements.ToArray());
            CurrentStatisticsActivity.Instance.BroadcastDetails("Total files analysed " + retValue);
            Thread.Sleep(500);
            CurrentStatisticsActivity.Instance.SetAndBroadcast(snapshotId, ActivityStatus.Stopped);
            Thread.Sleep(500);
            CurrentStatisticsActivity.Instance.ClearAndBroadcast();
        }

        private List<Element> GetListOfElementsForFolder(string folder, int snapshotId)
        {
            CurrentStatisticsActivity.Instance.BroadcastDetails(folder);
            var newElements = new List<Element>();
            foreach (string file in Directory.GetFiles(folder))
            {
                var musicFileFlag = Utils.GetMusicFlag(file, _pattern, Utils.Mp3MusicFile);
                newElements.Add(new Element
                {
                    Name = file,
                    SnapshotId = snapshotId,
                    MusicFileFlag = musicFileFlag,
                    LyricsFileFlag = ((musicFileFlag&MusicFileFlag.IsMusicFile) != MusicFileFlag.IsMusicFile) ? LyricsFileFlag.NoLyricsFile : Utils.GetLyricsFlag(file, _pattern, _musicSourceFolder, _lyricsSourceFolder),
                    IndexedFlag = ((musicFileFlag & MusicFileFlag.IsMusicFile) != MusicFileFlag.IsMusicFile) ? IndexedFlag.NotIndexed : Utils.GetIndexedFlag(file, _resultsProvider)
                });
            }
            return newElements;
        }
    }
}
