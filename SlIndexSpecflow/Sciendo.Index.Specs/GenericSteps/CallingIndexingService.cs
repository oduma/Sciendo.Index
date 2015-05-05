using Sciendo.Index.Specs.MusicClient;
using TechTalk.SpecFlow;

namespace Sciendo.Index.Specs.GenericSteps
{
    [Binding]
    public class CallingIndexingService
    {
        [When(@"I call the indexOnDemandService for Music")]
        public void WhenICallTheIndexOnDemandServiceForMusic()
        {
            IMusic musicClient = new MusicClient.MusicClient();
            var result = musicClient.IndexMusicOnDemand(ScenarioContext.Current["file"].ToString());
            ScenarioContext.Current.Add("result",result);
        }

        [When(@"I call the indexOnDemandService for Lyrics '(.*)'")]
        public void WhenICallTheIndexOnDemandServiceForLyrics(string p0)
        {
            IMusic musicClient = new MusicClient.MusicClient();
            var result = musicClient.IndexLyricsOnDemand(p0);
            ScenarioContext.Current.Add("result", result);
        }

    }
}
