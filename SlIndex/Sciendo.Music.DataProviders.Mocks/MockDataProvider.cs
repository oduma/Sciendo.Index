using System.Collections.Generic;
using System.Globalization;
using Sciendo.Music.DataProviders.Models.Indexing;
using Sciendo.Music.Contracts.MusicService;

namespace Sciendo.Music.DataProviders.Mocks
{
    public sealed class MockDataProvider:IDataProvider
    {
        
        public string[] GetMuiscAutocomplete(string term)
        {
            if(term == "c:\\preroot\\root\\")
            return new[]
            {
                "c:\\preroot\\root\\",
                "c:\\preroot\\root\\abc",
                "c:\\preroot\\root\\adc",
                "c:\\preroot\\root\\def",
                "c:\\preroot\\root\\fed",
                "c:\\preroot\\root\\abbcaccc.mp3",
                "c:\\preroot\\root\\soooa.ogg",
                "c:\\preroot\\root\\def1",
                "c:\\preroot\\root\\def2"
            };
            if (term == "c:\\preroot\\root\\abc\\")
            {
                var listofThings = new List<string>();
                listofThings.Add("c:\\preroot\\root\\abc\\");
                listofThings.Add("c:\\preroot\\root\\abc\\a1");
                listofThings.Add("c:\\preroot\\root\\abc\\a2");
                for (int i = 1; i <= 600; i++)
                {
                    listofThings.Add("c:\\preroot\\root\\abc\\" + i + "file.mp3");
                }
                return listofThings.ToArray();
            }
            return null;
        }

        public string[] GetLyricsAutocomplete(string term)
        {
            return GetMuiscAutocomplete(term);
        }

        public SourceFolders GetSourceFolders()
        {
            return new SourceFolders {Music = "c:\\preroot\\root".Replace(@"\","/"), Lyrics = "not avaialble".Replace(@"\","/")};
        }

        public IndexingResult StartIndexing(string fromPath, IndexType indexType)
        {
            if (indexType == IndexType.Music)
            {
                return new IndexingResult { IndexType = indexType.ToString(), NumberOfDocuments = 20.ToString(CultureInfo.InvariantCulture) };
            }
            return new IndexingResult { IndexType = indexType.ToString(), Error = "Lyrics indexing not available." };
        }

        public ProgressStatusModel[] GetMonitoring()
        {
            return new[]
            {
                new ProgressStatusModel {Id = "Id1", Package = "Package1", Status = "Done"},
                new ProgressStatusModel {Id = "Id2", Package = "Package2", Status = "Error"}
            };
        }

        public IndexingResult StartAcquyringLyrics(string fromPath, bool retryExisting)
        {
            return new IndexingResult {NumberOfDocuments = "1"};
        }

        public void Dispose()
        {
        }
    }
}
