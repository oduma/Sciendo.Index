using System.Collections.Generic;
using System.Globalization;
using Sciendo.Music.DataProviders.Models.Indexing;
using Sciendo.Music.Contracts.MusicService;
using System;

namespace Sciendo.Music.DataProviders.Mocks
{
    public sealed class MockDataProvider:IDataProvider
    {
        
        public string[] GetIndexingAutocomplete(string term)
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

        public string GetSourceFolder()
        {
            return "c:\\preroot\\root".Replace(@"\","/");
        }

        public void StartIndexing(string fromPath, Action<object,IndexOnDemandCompletedEventArgs> indexMusicCompletedCallback)
        {
            if (indexMusicCompletedCallback != null)
                indexMusicCompletedCallback(this, new IndexOnDemandCompletedEventArgs(new object [1] {20}, null, false, null));
        }

        public ProgressStatusModel[] GetMonitoring()
        {
            return new[]
            {
                new ProgressStatusModel {Id = "Id1", Package = "Package1", Status = "Done"},
                new ProgressStatusModel {Id = "Id2", Package = "Package2", Status = "Error"}
            };
        }
        


        public void StartAcquyringLyrics(string fromPath, bool retryExisting,Action<object, AcquireLyricsOnDemandForCompletedEventArgs> acquireLyricsCallback)
        {
                if (acquireLyricsCallback != null)
                    acquireLyricsCallback(this, new AcquireLyricsOnDemandForCompletedEventArgs(new[] { new IndexingResult { NumberOfDocuments = "1" } }, null, false, null));
        }

        public void Dispose()
        {
        }
    }
}
