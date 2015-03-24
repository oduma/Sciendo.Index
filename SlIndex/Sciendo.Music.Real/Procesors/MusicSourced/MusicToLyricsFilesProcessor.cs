using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Id3;
using Sciendo.Common.Logging;
using Sciendo.Common.Serialization;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Monitoring;
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
            CurrentConfiguration = configSection.Lyrics;
            CurrentMusicConfiguration = configSection.Music;
            LyricsDeserializer = new LyricsDeserializer();
            LoggingManager.Debug("Music to Lyrics file processor constructed.");

        }

        public AgentConfigurationSource CurrentMusicConfiguration { get; set; }

        public ILyricsDeserializer LyricsDeserializer { get; set; }

        public bool RetryExisting { get; set; }

        public override void ProcessFilesBatch(IEnumerable<string> files, Action<Status, string> progressEvent,ProcessType processType)
        {
            LoggingManager.Debug("Starting process batch of files " + files.Count());
            _progressEvent = progressEvent;
            IEnumerable<string> subjectFiles;
            if(!RetryExisting)
            {
                subjectFiles = files.Where(f=> !File.Exists(Path.ChangeExtension(
                    f.Replace(CurrentMusicConfiguration.SourceDirectory, CurrentConfiguration.SourceDirectory), CurrentConfiguration.SearchPattern.Replace("*.",""))));
            }
            else
            {
                subjectFiles = files;
            }
            if (!subjectFiles.Any())
            {
                return;
            }
            var package = TransformFiles<LyricsResult>(subjectFiles, TransformToLyricsResult,processType).ToArray();
            Counter += package.Count(p => p != null);
            LoggingManager.Debug("Processed batch of " + package.Length + " files.");
        }

        private string GetUrl(string artist, string song)
        {
           return string.Format("http://lyrics.wikia.com/api.php?func=getSong&artist={0}&song={1}&fmt=xml", artist,
                        song);
        }

        private LyricsResult TransformToLyricsResult(SongInfo transformFrom, string file, ProcessType processType)
        {
            var artist = string.Join("", transformFrom.Artists[0].Replace(" ", "_").ToCharArray().Where(c => (int)c >= 32));
            var song = transformFrom.Title.Replace(" ", "_");

            try
            {
                var downloadedFromApi = WebClient.TryQuery<string>(GetUrl(artist,song));
                var result = LyricsDeserializer.Deserialize<LyricsResult>(downloadedFromApi);
                var fullTargetPath = Path.ChangeExtension(
                    file.Replace(CurrentMusicConfiguration.SourceDirectory, CurrentConfiguration.SourceDirectory), CurrentConfiguration.SearchPattern.Replace("*.",""));
                var directoryPath = fullTargetPath.Replace(Path.GetFileName(fullTargetPath), "");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                using (StreamWriter fs = File.CreateText(fullTargetPath))
                {
                    fs.Write(downloadedFromApi);
                    if(_progressEvent!=null)
                        _progressEvent(Status.LyricsDownloadedOk,downloadedFromApi);
                    return result;
                }
            }
            catch (PreSerializationCheckException pcex)
            {
                if (_progressEvent != null)
                    _progressEvent(Status.Error, pcex.Message);
            }
            catch (Exception ex)
            {
                if (_progressEvent != null)
                    _progressEvent(Status.Error, ex.Message);

            }
            return null;
        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<SongInfo, string, ProcessType, T> specfifcTranformMethod,ProcessType processType)
        {
            return files.Select(file => specfifcTranformMethod(new SongInfo(new Mp3File(file)), file,processType));
        }
    }
}
