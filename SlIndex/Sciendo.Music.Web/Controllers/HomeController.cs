using System.Web.Mvc;
using Sciendo.Music.DataProviders;
using Sciendo.Music.DataProviders.Models.Indexing;

namespace Sciendo.Music.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IndexModel indexModel= new IndexModel(SciendoConfiguration.Container.Resolve<IDataProvider>(
                SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                .GetSourceFolder());

            return View("Index", indexModel);
        }

        [HttpGet]
        public JsonResult GetIndexingAutoComplete(string term)
        {
            return
                Json(SciendoConfiguration.Container.Resolve<IDataProvider>(
                        SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                        .GetIndexingAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}