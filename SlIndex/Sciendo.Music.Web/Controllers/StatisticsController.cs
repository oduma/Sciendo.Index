using Sciendo.Music.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sciendo.Music.Web.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            var statisticsModel = SciendoConfiguration.Container.Resolve<IStatisticsProvider>(
                SciendoConfiguration.StatisticsConfiguration.CurrentStatisticsProvider)
                .GetStatisticsModel();
            statisticsModel.FeedbackUrl = SciendoConfiguration.StatisticsConfiguration.FeedbackUrl;
            return View("StatisticsView",statisticsModel);
        }

        [HttpGet]
        public JsonResult GetSnapshotDetails(int id)
        {
            var snapshotDetails = SciendoConfiguration.Container.Resolve<IStatisticsProvider>(
                SciendoConfiguration.StatisticsConfiguration.CurrentStatisticsProvider)
                .GetSnapshotDetails("",id);

            return Json(snapshotDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TakeNew(string name)
        {
            var snapshot = SciendoConfiguration.Container.Resolve<IStatisticsProvider>(
                SciendoConfiguration.StatisticsConfiguration.CurrentStatisticsProvider)
                .TakeNewSnapshot(name);

            return Json(snapshot,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFolderDetails(string folderName, int id)
        {
            var snapshotDetails = SciendoConfiguration.Container.Resolve<IStatisticsProvider>(
                SciendoConfiguration.StatisticsConfiguration.CurrentStatisticsProvider)
                .GetSnapshotDetails(folderName, id);

            return Json(snapshotDetails, JsonRequestBehavior.AllowGet);
        }
    }
}