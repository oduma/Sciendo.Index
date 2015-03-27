using Sciendo.Index.Specs.MusicClient;
using TechTalk.SpecFlow;

namespace Sciendo.Index.Specs.GenericSteps
{
    [Binding]
    public class GivenIndexes
    {
        [Given(@"The file '(.*)' is not indexed yet")]
        public void GivenTheFileIsNotIndexedYet(string p0)
        {
            IMusic svc = new MusicClient.MusicClient();

            svc.UnIndexMusicOnDemand(p0);
            ScenarioContext.Current.Add("file",p0);
        }

    }
}
