using System.Web.Mvc;
using Sciendo.Music.DataProviders;
using Sciendo.Music.DataProviders.Common;

namespace Sciendo.Music.Web.Controllers
{
    public class QueryController : Controller
    {

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult BuildPlaylist()
        {
            var model =SciendoConfiguration.Container.Resolve<IPlaylistProvider>(
                SciendoConfiguration.PlaylistConfiguration.CurrentPlaylistProvider)
                .GetFullDocuments(1, SciendoConfiguration.PlaylistConfiguration.LastFmUser,
                    SciendoConfiguration.PlaylistConfiguration.LastFmBaseApiUrl,
                    SciendoConfiguration.PlaylistConfiguration.LastFmApiKey,SciendoConfiguration.Container.Resolve<IResultsProvider>(SciendoConfiguration.QueryConfiguration.CurrentDataProvider));
            return View("BuildPlaylist", model);
        }

        [HttpGet]
        public JsonResult Search(string criteria, int numRows,int startRow)
        {

            if (numRows == 0)
                numRows = SciendoConfiguration.QueryConfiguration.PageSize;
            return
                Json(
                    SciendoConfiguration.Container.Resolve<IResultsProvider>(
                        SciendoConfiguration.QueryConfiguration.CurrentDataProvider)
                        .GetResultsPackage(criteria,numRows,startRow, new SolrVagueQueryStrategy(criteria,numRows,startRow),WebRetriever.TryGet<SolrResponse>), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Filter(string criteria, int numRows, int startRow, string facetFieldName, string facetFieldValue)
        {
            if (numRows == 0)
                numRows = SciendoConfiguration.QueryConfiguration.PageSize;
            return
                Json(
                    SciendoConfiguration.Container.Resolve<IResultsProvider>(
                        SciendoConfiguration.QueryConfiguration.CurrentDataProvider)
                        .GetFilteredResultsPackage(criteria, numRows, startRow, facetFieldName, facetFieldValue,
                            new SolrVagueQueryStrategy(criteria, numRows, startRow, facetFieldName, facetFieldValue),WebRetriever.TryGet<SolrResponse>),
                    JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddSongToQueue(string filePath)
        {
            return
                Json(
                    SciendoConfiguration.Container.Resolve<IPlayerProcess>(
                        SciendoConfiguration.PlayerConfiguration.CurrentPlayerProcess)
                        .AddSongToQueue(filePath, SciendoConfiguration.PlayerConfiguration.PlayerProcessIdentifier), JsonRequestBehavior.AllowGet);
        }
    }
}
