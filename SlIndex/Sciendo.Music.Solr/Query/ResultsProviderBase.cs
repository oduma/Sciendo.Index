﻿using Sciendo.Music.Solr.Query.Common;
using Sciendo.Music.Solr.Query.FromSolr;
using Sciendo.Music.Solr.Query.ToConsummer;
using Sciendo.Music.Solr.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sciendo.Music.Solr.Query
{
    public abstract class ResultsProviderBase:IResultsProvider
    {
        protected Field[] GetFields(SolrResponse solrResponse)
        {
            if(solrResponse==null || solrResponse.facet_counts==null || solrResponse.facet_counts.FacetFields==null)
                return null;
            return new[] {new Field{Name="Artists", Values=GetFacetField(solrResponse.facet_counts.FacetFields.ArtistF).Where(a=>a!=null).ToArray()},
            new Field{Name="Extensions", Values=GetFacetField(solrResponse.facet_counts.FacetFields.ExtensionF).Where(a=>a!=null).ToArray()},
            new Field{Name="Letters", Values=GetFacetField(solrResponse.facet_counts.FacetFields.LetterCatalogF).Where(a=>a!=null).ToArray()}
        };
        }

        private IEnumerable<FieldValue> GetFacetField(object[] fieldFacet)
        {

            if (fieldFacet != null && fieldFacet.Any())
            {
                var newKey = string.Empty;
                var newValue = 0;
                for (var i = 0; i < fieldFacet.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        newKey = (string)((fieldFacet[i]) ?? "Unknown");
                        newValue = 0;
                    }
                    else
                        newValue = Convert.ToInt32(fieldFacet[i]);

                    if (newValue != 0)
                    {
                        yield return new FieldValue { Key = newKey, Count = newValue };
                        newKey = string.Empty;
                    }
                }
            }
            yield return null;
        }

        protected Doc[] ApplyHighlights(SolrResponse response)
        {
            if (response.highlighting == null)
                return response.response.Docs;

            return response.response.Docs.Join(response.highlighting, d => d.FilePathId, h => h.Key,
                (d, h) =>
                    new Doc
                    {
                        Album = (h.Value.Album == null) ? d.Album : h.Value.Album[0],
                        Artist = h.Value.Artist ?? d.Artist,
                        FilePathId = d.FilePathId,
                        Lyrics = (h.Value.Lyrics == null) ? d.Lyrics : h.Value.Lyrics[0],
                        Title = (h.Value.Title == null) ? d.Title : h.Value.Title[0],
                        FilePath = d.FilePath
                    }).ToArray();
        }

        protected PageInfo GetNewPageInfo (SolrResponse response, int numRowsRequested, int startRow)
        {
            return new PageInfo { TotalRows = response.response.NumFound, RowsPerPage = numRowsRequested, PageStartRow = startRow };
        }

        public abstract ResultsPackage GetResultsPackageWithPreciseStrategy(string query, int numRows, int startRow, RequestType requestType);

        public abstract ResultsPackage GetResultsPackageWithVagueStrategy(string query, int numRows, int startRow, RequestType requestType);

        public abstract ResultsPackage GeFilteredtResultsPackageWithVagueStrategy(string query, int numRows, int startRow, string facetFieldName, string faceFieldValue, RequestType requestType);
    }
}
