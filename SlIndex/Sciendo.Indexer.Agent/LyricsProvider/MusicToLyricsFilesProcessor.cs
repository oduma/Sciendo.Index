using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Sciendo.Common.Logging;
using Sciendo.Common.Serialization;
using Sciendo.Lyrics.Common;
using Sciendo.Music.Agent.Common;

namespace Sciendo.Music.Agent.LyricsProvider
{
    public class MusicToLyricsFilesProcessor:FilesProcessorBase<SongInfo>
    {
        private readonly WebDownloaderBase _webClient;
        private Action<Status, string> _progressEvent;

        public MusicToLyricsFilesProcessor()
        {
            var configSection = (AgentConfigurationSection)ConfigurationManager.GetSection("agent");
            _webClient = new WebDownloader();
            CurrentConfiguration = configSection.Lyrics;
            CurrentMusicConfiguration = configSection.Music;
            LyricsDeserializer = new LyricsDeserializer();

        }

        public AgentConfigurationSource CurrentMusicConfiguration { get; set; }

        public LyricsDeserializer LyricsDeserializer { get; set; }

        public bool RetryExisting { get; set; }

        public override void ProcessFilesBatch(IEnumerable<string> files, Action<Status, string> progressEvent)
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
            var package = TransformFiles<LyricsResult>(subjectFiles, TransformToLyricsResult).ToArray();
            Counter += package.Length;
            LoggingManager.Debug("Processed batch of " + package.Length + " files.");
        }

        private string GetUrl(string artist, string song)
        {
           return string.Format("http://lyrics.wikia.com/api.php?func=getSong&artist={0}&song={1}&fmt=xml", artist,
                        song);
        }

        private LyricsResult TransformToLyricsResult(SongInfo transformFrom, string file)
        {
            var artist = string.Join("", transformFrom.Artists[0].Replace(" ", "_").ToCharArray().Where(c => (int)c >= 32));
            var song = transformFrom.Title.Replace(" ", "_");

            try
            {
                var downloadedFromApi = _webClient.TryQuery<string>(GetUrl(artist,song));
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

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<SongInfo, string, T> specfifcTranformMethod)
        {
            return files.Select(file => specfifcTranformMethod(new SongInfo(file), file));
        }
    }
}
