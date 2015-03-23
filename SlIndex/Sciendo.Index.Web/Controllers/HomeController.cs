using System.Web.Mvc;
using Sciendo.Music.DataProviders;
using Sciendo.Music.DataProviders.Models;

namespace Sciendo.Index.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            IndexModel indexModel= new IndexModel(SciendoConfiguration.Container.Resolve<IDataProvider>(
                SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                .GetSourceFolders());


            return View("Index", indexModel);
        }

        [HttpGet]
        public JsonResult GetMusicAutoComplete(string term)
        {
            return
                Json(SciendoConfiguration.Container.Resolve<IDataProvider>(
                        SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                        .GetMuiscAutocomplete(term), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLyricsAutoComplete(string term)
        {
            return
                Json(SciendoConfiguration.Container.Resolve<IDataProvider>(
                        SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                        .GetLyricsAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult StartIndexing(string fromPath,IndexType indexType)
        {
            return
                Json(SciendoConfiguration.Container.Resolve<IDataProvider>(
                        SciendoConfiguration.IndexingConfiguration.CurrentDataProvider)
                        .StartIndexing(fromPath,indexType), JsonRequestBehavior.AllowGet);
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