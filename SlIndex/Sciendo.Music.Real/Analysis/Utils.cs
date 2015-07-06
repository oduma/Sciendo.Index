using Id3;
using Id3.Id3;
using Sciendo.Common.Logging;
using Sciendo.Common.Serialization;
using Sciendo.Music.Contracts.Analysis;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Real.Lyrics.Provider;
using Sciendo.Music.Solr;
using Sciendo.Music.Solr.Query;
using Sciendo.Music.Solr.Query.FromSolr;
using Sciendo.Music.Solr.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sciendo.Music.Real.Analysis
{
    public static class Utils
    {
        public static readonly string _lrcExtension = ".lrc";

        public static IMp3Stream Mp3MusicFile(string file)
        {
            return new Mp3File(file);
        }
        public static MusicFileFlag GetMusicFlag(string file,string pattern,Func<string,IMp3Stream> musicFile)
        {
            if (pattern.Split('|').All(p => !file.ToLower().EndsWith(p.Replace("*",""))))
                return MusicFileFlag.NotAMusicFile;
            var mp3File = musicFile(file);
            
            if (mp3File.HasTags && mp3File.AvailableTagVersions != null)
            {
                var version = Enumerable.FirstOrDefault<Version>(mp3File.AvailableTagVersions);
                if (version != null)
                {
                    IId3Tag id3Tag = null;
                    try
                    {
                        id3Tag = mp3File.GetTag(version.Major, version.Minor);
                    }
                    catch
                    {
                        return MusicFileFlag.IsMusicFile;
                    }
                    if (id3Tag != null)
                    {
                        var flags = MusicFileFlag.IsMusicFile|MusicFileFlag.HasTag;
                            
                        if (Enumerable.Select<string, string>(id3Tag.Artists.Value,
                            a => string.Join("", Enumerable.Where<char>(a.ToCharArray(), c => ((int)c) >= 32))).Any(a => !string.IsNullOrEmpty(a)))
                            flags = flags | MusicFileFlag.HasArtistTag;
                        if (!string.IsNullOrEmpty(id3Tag.Title.TextValue))
                            flags = flags | MusicFileFlag.HasTitleTag;
                        if (!string.IsNullOrEmpty(id3Tag.Album.TextValue))
                            flags = flags | MusicFileFlag.HasAlbumTag;
                        return flags;
                    }
                    else
                        return MusicFileFlag.IsMusicFile;
                }
                else
                    return MusicFileFlag.IsMusicFile;
            }
            else
            {
                return MusicFileFlag.IsMusicFile;
            }
        }

        public static LyricsFileFlag GetLyricsFlag(string file,string pattern, string musicSourceFolder, string lyricsSourceFolder)
        {
            if (File.Exists(@"TestData\Lyrics\Sub1\MockOggWithoutLyrics.lrc"))
                File.Delete(@"TestData\Lyrics\Sub1\MockOggWithoutLyrics.lrc");
            var musicExtension = pattern.Split('|').FirstOrDefault(p => file.ToLower().EndsWith(p.Replace("*", "")));
            if (musicExtension == null)
                return LyricsFileFlag.NoLyricsFile;
            musicExtension = musicExtension.Replace("*", "");
            var lyricsFileName = file.Replace(musicExtension, _lrcExtension).Replace(musicSourceFolder,lyricsSourceFolder);
            if (!File.Exists(lyricsFileName))
                return LyricsFileFlag.NoLyricsFile;
            LyricsFileFlag flags = LyricsFileFlag.HasLyricsFile;
            string lyrics = File.ReadAllText(lyricsFileName);
            LyricsDeserializer lyricsDeserializer = new LyricsDeserializer(); 
            try
            {
                var result = lyricsDeserializer.Deserialize<LyricsResult>(lyrics);
                if(result.lyrics=="Not found")
                {
                    flags = flags | LyricsFileFlag.LyricsFileNoLyrics;
                    return flags;
                }
                flags = flags | LyricsFileFlag.LyricsFileOk;
                return flags;
            }
            catch (PreSerializationCheckException pcex)
            {
                flags = flags | LyricsFileFlag.LyricsFileWithError;
                return flags;
            }
            catch (Exception ex)
            {
                flags = flags | LyricsFileFlag.LyricsFileWithError;
                return flags;
            }
        }

        public static IndexedFlag GetIndexedFlag(string file,IResultsProvider resultsProvider)
        {
            var solrQuery = string.Format("file_path_id:\"{0}\"",file);

            try
            {
                var results =
                    resultsProvider.GetResultsPackageWithPreciseStrategy(solrQuery, 1, 0, RequestType.Post);
                if (results == null || results.ResultRows == null || !results.ResultRows.Any())
                    return IndexedFlag.NotIndexed;
                if (results.ResultRows[0] == null)
                    return IndexedFlag.NotIndexed;
                IndexedFlag flags=IndexedFlag.Indexed;
                if (!string.IsNullOrEmpty(results.ResultRows[0].Album))
                    flags = flags | IndexedFlag.IndexedAlbum;
                if (results.ResultRows[0].Artist!=null && results.ResultRows[0].Artist.Any(a=>!string.IsNullOrEmpty(a)))
                    flags = flags | IndexedFlag.IndexedArtist;
                if (!string.IsNullOrEmpty(results.ResultRows[0].Title))
                    flags = flags | IndexedFlag.IndexedTitle;
                if (!string.IsNullOrEmpty(results.ResultRows[0].Lyrics))
                    flags = flags | IndexedFlag.IndexedLyrics;
                return flags;
            }
            catch(Exception ex)
            {
                LoggingManager.LogSciendoSystemError(ex);
                return IndexedFlag.NotIndexed;
            }
            
        }
    }
}
