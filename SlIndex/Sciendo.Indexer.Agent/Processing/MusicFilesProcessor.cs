using System;
using System.Collections.Generic;
using System.Configuration;
using Sciendo.Common.Logging;
using Sciendo.Music.Agent.Common;
using Sciendo.Music.Agent.Service.Solr;

namespace Sciendo.Music.Agent.Processing
{
    public class MusicFilesProcessor:FilesProcessor<SongInfo>
    {
        public MusicFilesProcessor()
        {
            LoggingManager.Debug("Constructing MusicFilesProcessor...");
            var configSection = (AgentConfigurationSection) ConfigurationManager.GetSection("indexer");
            Sender=new SolrSender(configSection.SolrConnectionString);
            CurrentConfiguration = configSection.Music;
            LoggingManager.Debug("MusicFilesProcessor constructed.");
        }

        protected override IEnumerable<T> TransformFiles<T>(IEnumerable<string> files,Func<SongInfo,string, T> specificTransfromFunction )
        {
            LoggingManager.Debug("MusicFileProcessor preparing documents...");
            if(typeof(T)!=typeof(Document))
                LoggingManager.LogSciendoSystemError("Wrong type for this processor",new Exception());
            foreach (var file in files)
            {
                var songInfo = new SongInfo(file);

                yield return specificTransfromFunction(songInfo,file);
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
