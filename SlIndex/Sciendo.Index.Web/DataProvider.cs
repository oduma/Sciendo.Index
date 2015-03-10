﻿using System;
using Sciendo.Index.Web.IndexingClient;
using Sciendo.Index.Web.Models;

namespace Sciendo.Index.Web
{
    public class DataProvider:IDataProvider
    {
        IIndexerAgent _svc = new IndexerAgentClient();

        public string[] GetMuiscAutocomplete(string term)
        {
            
            return _svc.ListAvailableMusicPathsForIndexing(term);
        }

        public string[] GetLyricsAutocomplete(string term)
        {
            return _svc.ListAvailableLyricsPathsForIndexing(term);
        }

        public SourceFolders GetSourceFolders()
        {
            return _svc.GetSourceFolders();
        }

        public IndexingResult StartIndexing(string fromPath, IndexType indexType)
        {
            try
            {
                switch (indexType)
                {
                    case IndexType.Music:
                        return new IndexingResult
                        {
                            IndexType = indexType.ToString(),
                            NumberOfDocuments = _svc.IndexMusicOnDemand(fromPath).ToString()
                        };
                    case IndexType.Lyrics:
                        return new IndexingResult
                        {
                            IndexType = indexType.ToString(),
                            NumberOfDocuments = _svc.IndexLyricsOnDemand(fromPath).ToString()
                        };
                    default:
                        return new IndexingResult
                        {
                            IndexType = IndexType.None.ToString(),
                            NumberOfDocuments = 0.ToString(),
                            Error = "Index Type unknown."
                        };
                }

            }
            catch (Exception ex)
            {
                return new IndexingResult
                {
                    IndexType = indexType.ToString(),
                    NumberOfDocuments = 0.ToString(),
                    Error = ex.Message
                };
            }
        }
    }
}
