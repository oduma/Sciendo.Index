using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Id3;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Common;
using Sciendo.Music.Contracts.Solr;
using Sciendo.Music.Real.Lyrics.Provider;
using Sciendo.Music.Real.Procesors.Common;
using Sciendo.Music.Real.Procesors.Configuration;
using Sciendo.Music.Real.Solr;

namespace Sciendo.Music.Real.Procesors.MusicSourced
{
    public class IndexingFilesProcessor:FilesProcessor<SongInfo>
    {
        private MusicToLyricsFilesProcessor _lyricsAquirer;

        public IndexingFilesProcessor()
        {
            LoggingManager.Debug("Constructing IndexingFilesProcessor...");
            var configSection = (AgentConfigurationSection) ConfigurationManager.GetSection("agent");
            Sender=new SolrSender(configSection.SolrConnectionString);
            CurrentConfiguration = configSection;
            LyricsDeserializer = new LyricsDeserializer();
            _lyricsAquirer = new MusicToLyricsFilesProcessor();
            LoggingManager.Debug("IndexingFilesProcessor constructed.");
        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<SongInfo, string, T> specificTransfromFunction)
        {
            LoggingManager.Debug("MusicFileProcessor preparing documents...");
            if(typeof(T)!=typeof(Document))
                LoggingManager.LogSciendoSystemError("Wrong type for this processor",new Exception());
            foreach (var file in files)
            {

                if (!File.Exists(file)) // in case of file not existing it means that it is been deleted
                {
                    DeletedDocuments.Add(new DeleteDocument(file));
                    DeletedDocuments.Add(new DeleteDocument(GetLyricsFile(file)));
                }
                else
                    yield return specificTransfromFunction(new SongInfo(new Mp3File(file)), file);
            }
            LoggingManager.Debug("MusicFileProcessor documents prepared.");
        }

        protected override Document TransformToDocument(SongInfo songInfo, string file)
        {
            var lyricsFile = GetLyricsFile(file);
            string lyrics = string.Empty;
            if (!File.Exists(lyricsFile))
            {
                var lyricsResult = _lyricsAquirer.GetLyricsResult(songInfo, lyricsFile);
                if (lyricsResult != null)
                    lyrics = lyricsResult.lyrics;
            }
            else
                lyrics = GetLyricsForMusicFile(lyricsFile);

            return new Document(file, CatalogLetter(file, CurrentConfiguration.Music.SourceDirectory), songInfo.Artists,
                songInfo.Title, songInfo.Album,lyrics);
        }

        private string GetLyricsForMusicFile(string lyricsFile)
        {
            LoggingManager.Debug("Getting lyrics from file: " + lyricsFile);
            if (File.Exists(lyricsFile))
            {
                var lyricsResult = LyricsDeserializer.DeserializeFromFile<LyricsResult>(lyricsFile);
                if (lyricsResult != null)
                {
                    return lyricsResult.lyrics;
                }
                return string.Empty;
            }
            
            return string.Empty;
        }

        private string GetLyricsFile(string file)
        {
            LoggingManager.Debug("Get Lyrics File for music file: " + file);
            var lyricsFile = file.Replace(CurrentConfiguration.Music.SourceDirectory, CurrentConfiguration.Lyrics.SourceDirectory);
                
                lyricsFile = Path.Combine(Path.GetDirectoryName(lyricsFile),
                                    Path.GetFileNameWithoutExtension(lyricsFile) + ".lrc");
                LoggingManager.Debug("Lyrics file: " + lyricsFile);
            return lyricsFile;
        }
    }
}
