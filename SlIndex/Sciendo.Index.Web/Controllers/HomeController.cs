using System.Web.Mvc;

namespace Sciendo.Index.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetMusicAutoComplete(string term)
        {
            return
                Json(
                    new string[3] {"abc","adc","afc"}, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLyricsAutoComplete(string term)
        {
            return
                Json(
                    new string[3] { "abc", "adc", "afc" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Monitor()
        {
            return View();
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

    public class SearchResult
    {
        public string label { get; set; }
        public string value { get; set; }
    }
}