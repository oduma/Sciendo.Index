using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Id3;
using Sciendo.Common.Logging;
using Sciendo.Common.Serialization;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Processing;
using Sciendo.Music.Real.Lyrics.Provider;
using Sciendo.Music.Real.Procesors.Common;
using Sciendo.Music.Real.Procesors.Configuration;

namespace Sciendo.Music.Real.Procesors.MusicSourced
{
    public class MusicToLyricsFilesProcessor:FilesProcessorBase<SongInfo>
    {
        protected WebDownloaderBase WebClient;
        private Action<Status, string> _progressEvent;

        public MusicToLyricsFilesProcessor()
        {
            LoggingManager.Debug("Constructing Music to Lyrics file processor...");
            var configSection = (AgentConfigurationSection)ConfigurationManager.GetSection("agent");
            WebClient = new WebDownloader();
            CurrentConfiguration = configSection;
            LyricsDeserializer = new LyricsDeserializer();
            LoggingManager.Debug("Music to Lyrics file processor constructed.");

        }


        public bool RetryExisting { get; set; }

        public override void ProcessFilesBatch(IEnumerable<string> files, Action<Status, string> progressEvent)
        {
            LoggingManager.Debug("Starting process batch of files " + files.Count());
            _progressEvent = progressEvent;
            IEnumerable<string> subjectFiles;
            if(!RetryExisting)
            {
                subjectFiles = files.Where(f=> !File.Exists(Path.ChangeExtension(
                    f.Replace(CurrentConfiguration.Music.SourceDirectory, CurrentConfiguration.Lyrics.SourceDirectory), CurrentConfiguration.Lyrics.SearchPattern.Replace("*.",""))));
            }
            else
            {
                subjectFiles = files;
            }
            if (!subjectFiles.Any())
            {
                return;
            }
            var package = TransformFiles<LyricsResult>(subjectFiles, TransformToLyricsResult).ToArray();
            Counter += package.Count(p => p != null);
            LoggingManager.Debug("Processed batch of " + package.Length + " files.");
        }

        private string GetUrl(string artist, string song)
        {
           return string.Format("http://lyrics.wikia.com/api.php?func=getSong&artist={0}&song={1}&fmt=xml", artist,
                        song);
        }

        private LyricsResult TransformToLyricsResult(SongInfo songInfo, string file)
        {
            if (songInfo == null || string.IsNullOrEmpty(songInfo.Title) || songInfo.Artists.Length <= 0)
                return null;
            var fullTargetPath = Path.ChangeExtension(
                    file.Replace(CurrentConfiguration.Music.SourceDirectory, CurrentConfiguration.Lyrics.SourceDirectory), CurrentConfiguration.Lyrics.SearchPattern.Replace("*.",""));
            return GetLyricsResult(songInfo, fullTargetPath);
        }

        public LyricsResult GetLyricsResult(SongInfo songInfo, string lyricsFile)
        {
            var directoryPath = lyricsFile.Replace(Path.GetFileName(lyricsFile), "");

            if (songInfo == null) //assume the music file has been deleted so try to delete the lyrics file
                DeleteLyrics(lyricsFile);

            var artist = string.Join("", songInfo.Artists[0].Replace(" ", "_").ToCharArray().Where(c => (int) c >= 32));
            var song = songInfo.Title.Replace(" ", "_");
            string downloadedFromApi = string.Empty;
            try
            {
                LoggingManager.Debug("Trying to get lyrics from the url: " + GetUrl(artist, song));
                downloadedFromApi = WebClient.TryQuery<string>(GetUrl(artist, song));
                var result = LyricsDeserializer.Deserialize<LyricsResult>(downloadedFromApi);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                using (StreamWriter fs = File.CreateText(lyricsFile))
                {
                    fs.Write(downloadedFromApi);
                    if (_progressEvent != null)
                        _progressEvent(Status.LyricsDownloadedOk,  downloadedFromApi);
                    return result;
                }
            }
            catch (PreSerializationCheckException pcex)
            {
                LoggingManager.LogSciendoSystemError(downloadedFromApi, pcex);
                if (_progressEvent != null)
                    _progressEvent(Status.Error, pcex.Message);
            }
            catch (Exception ex)
            {
                LoggingManager.LogSciendoSystemError(downloadedFromApi, ex);
                if (_progressEvent != null)
                    _progressEvent(Status.Error, ex.Message);
            }
            return null;
        }

        private void DeleteLyrics(string fullTargetPath)
        {
            if(File.Exists(fullTargetPath))
                File.Delete(fullTargetPath);
        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<SongInfo, string, T> specfifcTranformMethod)
        {
            return files.Select(file => specfifcTranformMethod((!File.Exists(file))?null:new SongInfo(new Mp3File(file)), file));
        }
    }
}
