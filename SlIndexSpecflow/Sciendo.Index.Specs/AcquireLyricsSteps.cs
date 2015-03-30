using System;
using Sciendo.Index.Specs.MusicClient;
using TechTalk.SpecFlow;

namespace Sciendo.Index.Specs
{
    [Binding]
    public class AcquireLyricsSteps
    {

        [When(@"I acquire the lyrics for '(.*)'")]
        public void WhenIAcquireTheLyricsFor(string p0)
        {
            IMusic svc= new MusicClient.MusicClient();
            var response = svc.AcquireLyricsOnDemandFor(p0, false);
            ScenarioContext.Current.Add("result",response);
        }

    }
}
