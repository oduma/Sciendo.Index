using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Sciendo.Index.Specs
{
    [Binding]
    public class IndexingOfMusicFilesSteps
    {


        [Then(@"the result should be (.*)")]
public void ThenTheResultShouldBe(int p0)
{
    Assert.AreEqual(p0,ScenarioContext.Current["result"]);
}
    }
}
