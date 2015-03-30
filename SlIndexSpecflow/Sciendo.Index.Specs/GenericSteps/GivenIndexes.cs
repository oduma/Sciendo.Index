using Sciendo.Index.Specs.MusicClient;
using TechTalk.SpecFlow;

namespace Sciendo.Index.Specs.GenericSteps
{
    [Binding]
    public class GivenIndexes
    {
        [Given(@"The music file '(.*)' is not indexed yet")]
        public void GivenTheMusicFileIsNotIndexedYet(string p0)
        {
            IMusic svc = new MusicClient.MusicClient();

            svc.UnIndexMusicOnDemand(p0);
            ScenarioContext.Current.Add("file",p0);
        }

        [Given(@"The lyrics file '(.*)' is not indexed yet")]
        public void GivenTheLyricsFileIsNotIndexedYet(string p0)
        {
            IMusic svc = new MusicClient.MusicClient();

            svc.UnIndexLyricsOnDemand(p0);
            ScenarioContext.Current.Add("file", p0);
        }

    }
}
