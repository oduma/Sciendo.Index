using System.Collections.Generic;
using Sciendo.Index.Web.IndexingClient;

namespace Sciendo.Index.Web
{
    public class MockDataProvider:IDataProvider
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
                "c:\\preroot\\root\\def2",
            };
            else if (term == "c:\\preroot\\root\\abc\\")
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
            throw new System.NotImplementedException();
        }

        public SourceFolders GetSourceFolders()
        {
            return new SourceFolders {Music = "c:\\preroot\\root".Replace(@"\","/"), Lyrics = "not avaialble".Replace(@"\","/")};
        }
    }
}
