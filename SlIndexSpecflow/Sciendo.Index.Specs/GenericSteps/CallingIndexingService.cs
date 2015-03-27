using Sciendo.Index.Specs.MusicClient;
using TechTalk.SpecFlow;

namespace Sciendo.Index.Specs.GenericSteps
{
    [Binding]
    public class CallingIndexingService
    {
        [When(@"I call the indexOnDemandService")]
        public void WhenICallTheIndexOnDemandService()
        {
            IMusic musicClient = new MusicClient.MusicClient();
            var result = musicClient.IndexMusicOnDemand(ScenarioContext.Current["file"].ToString());
            ScenarioContext.Current.Add("result",result);
        }

    }
}
