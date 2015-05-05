using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Id3;
using Sciendo.Common.Logging;
using Sciendo.Music.Contracts.Solr;
using Sciendo.Music.Real.Procesors.Common;
using Sciendo.Music.Real.Procesors.Configuration;
using Sciendo.Music.Real.Solr;

namespace Sciendo.Music.Real.Procesors.MusicSourced
{
    public class MusicFilesProcessor:FilesProcessor<SongInfo>
    {
        public MusicFilesProcessor()
        {
            LoggingManager.Debug("Constructing MusicFilesProcessor...");
            var configSection = (AgentConfigurationSection) ConfigurationManager.GetSection("agent");
            Sender=new SolrSender(configSection.SolrConnectionString);
            CurrentConfiguration = configSection.Music;
            LoggingManager.Debug("MusicFilesProcessor constructed.");
        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files, Func<SongInfo, string, T> specificTransfromFunction)
        {
            LoggingManager.Debug("MusicFileProcessor preparing documents...");
            if(typeof(T)!=typeof(Document))
                LoggingManager.LogSciendoSystemError("Wrong type for this processor",new Exception());
            foreach (var file in files)
            {
                
                if(!File.Exists(file)) // in case of file not existing it means that it is been deleted
                    DeletedDocuments.Add(new DeleteDocument(file));
                else
                    yield return specificTransfromFunction(new SongInfo(new Mp3File(file)), file);
            }
            LoggingManager.Debug("MusicFileProcessor documents prepared.");
        }

        protected override Document TransformToDocument(SongInfo songInfo, string file)
        {
            return new FullDocument(file, CatalogLetter(file, CurrentConfiguration.SourceDirectory), songInfo.Artists,
                songInfo.Title, songInfo.Album);
        }
    }
}
