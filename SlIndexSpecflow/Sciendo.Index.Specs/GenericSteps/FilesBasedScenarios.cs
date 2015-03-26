using System.IO;
using TechTalk.SpecFlow;

namespace Sciendo.Index.Specs.GenericSteps
{
    [Binding]
    public class FilesBasedScenarios
    {
        [Given(@"The file '(.*)' exists")]
        public void GivenTheFileExists(string p0)
        {
            if (!File.Exists(p0))
                File.Create(p0);
            ScenarioContext.Current.Add("file",p0);
        }
    }
}
