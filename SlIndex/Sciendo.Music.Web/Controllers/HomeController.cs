using System.Web.Mvc;
using Sciendo.Music.DataProviders;
using Sciendo.Music.DataProviders.Models.Indexing;

namespace Sciendo.Music.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var sourceFolder = SciendoConfiguration.Container.Resolve<IDataProvider>(
                SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                .GetSourceFolder();
            IndexModel indexModel = new IndexModel();
            if (string.IsNullOrEmpty(sourceFolder))
                indexModel.ErrorMessage = "Agent not available.";
            else
                indexModel.SourceFolder = sourceFolder;

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

        [HttpGet]
        public JsonResult StartIndexing(string fromPath)
        {
            
            SciendoConfiguration.Container.Resolve<IDataProvider>(SciendoConfiguration.IndexingConfiguration.CurrentDataProvider).StartIndexing(fromPath);
            return Json("yes", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult StartAcquireLyrics(string fromPath,bool retry)
        {
            SciendoConfiguration.Container.Resolve<IDataProvider>(SciendoConfiguration.IndexingConfiguration.CurrentDataProvider).StartAcquyringLyrics(fromPath,retry);
            return Json("yes", JsonRequestBehavior.AllowGet);

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