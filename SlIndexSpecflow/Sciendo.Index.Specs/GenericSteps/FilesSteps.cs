using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Sciendo.Index.Specs.MusicClient;
using TechTalk.SpecFlow;

namespace Sciendo.Index.Specs.GenericSteps
{
    [Binding]
    public class FilesSteps
    {
        [Given(@"I have no lyrics file '(.*)'")]
        public void GivenIHaveNoLyricsFile(string p0)
        {
            IMusic svc = new MusicClient.MusicClient();
            svc.DeleteLyricsFile(p0);
            ScenarioContext.Current.Add("lyricsFile", p0);
        }

        [Then(@"the result is the file '(.*)' exists")]
        public void ThenTheResultIsTheFileExists(string p0)
        {
            Assert.True(File.Exists(p0));
        }

    }
}
