using Sciendo.Index.Specs.MusicClient;
using TechTalk.SpecFlow;

namespace Sciendo.Index.Specs
{
    [Binding]
    public class IndexingOfMusicFilesSteps
    {

        [When(@"I call the indexOnDemandService")]
public void WhenICallTheIndexOnDemandService()
{
            IMusic musicClient=new MusicClient.MusicClient();
            musicClient.IndexMusicOnDemand(ScenarioContext.Current["file"].ToString());
    ScenarioContext.Current.Pending();
}

        [Then(@"the result should be (.*)")]
public void ThenTheResultShouldBe(int p0)
{
    ScenarioContext.Current.Pending();
}
    }
}
