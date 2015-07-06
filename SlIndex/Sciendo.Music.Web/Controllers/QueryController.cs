using System;
using System.Web.Mvc;
using Sciendo.Music.DataProviders;
using Sciendo.Music.DataProviders.Models.Playlist;
using System.Linq;
using Sciendo.Music.Solr.Query;
using Sciendo.Music.Solr.Strategies;
using Sciendo.Music.Solr.Query.FromSolr;
using Sciendo.Music.Solr;

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
            return View("BuildPlaylist");
        }

        [HttpGet]
        public JsonResult RefreshPlaylist(string lastFmUserName)
        {
            if(string.IsNullOrEmpty(lastFmUserName))
            {
                lastFmUserName=SciendoConfiguration.PlaylistConfiguration.LastFmUser;
            }

            if(Session["currentplaylist"]!=null)
            {
                Session.Remove("currentplaylist");
            }
            var currentPlaylist = SciendoConfiguration.Container.Resolve<IPlaylistProvider>(
                SciendoConfiguration.PlaylistConfiguration.CurrentPlaylistProvider)
                .RefreshPlaylist(lastFmUserName,
                    SciendoConfiguration.PlaylistConfiguration.LastFmBaseApiUrl,
                    SciendoConfiguration.PlaylistConfiguration.LastFmApiKey,
                    SciendoConfiguration.Container.Resolve<IResultsProvider>(
                        SciendoConfiguration.QueryConfiguration.CurrentDataProvider));
            Session.Add("currentplaylist", currentPlaylist);

            return Json(currentPlaylist.Pages[0], JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPlaylistPage(string lastFmUserName, int numRows, int startRow)
        {
            int pageNo;
            if (numRows == 0)
            {
                pageNo = 0;
            }
            else
            {
                pageNo = startRow / numRows;
            }

            if (Session["currentplaylist"] == null)
            {
                throw new Exception("No Playlist on the server!");
            }
            var playlist = ((PlaylistModel) Session["currentplaylist"]);
            if (playlist.Pages.Count < pageNo)
                return Json(((PlaylistModel) Session["currentplaylist"]).Pages[pageNo], JsonRequestBehavior.AllowGet);

            playlist.Pages.Add(SciendoConfiguration.Container.Resolve<IPlaylistProvider>(
                SciendoConfiguration.PlaylistConfiguration.CurrentPlaylistProvider)
                .GetNewPlaylistPage(pageNo,lastFmUserName,
                    SciendoConfiguration.PlaylistConfiguration.LastFmBaseApiUrl,
                    SciendoConfiguration.PlaylistConfiguration.LastFmApiKey,
                    SciendoConfiguration.Container.Resolve<IResultsProvider>(
                        SciendoConfiguration.QueryConfiguration.CurrentDataProvider)));
            Session["currentplaylist"]=playlist;

            return Json(playlist.Pages[pageNo], JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChangeItemInPlaylist(string playlistItem, bool onOff, int numRows, int startRow)
        {
            int pageNo;
            if (numRows == 0)
            {
                pageNo = 0;
            }
            else
            {
                pageNo = startRow / numRows;
            }
            if (string.IsNullOrEmpty(playlistItem) || Session["currentplaylist"] == null)
            {
                return Json(((PlaylistModel) Session["currentplaylist"]).Pages[pageNo], JsonRequestBehavior.AllowGet);

            }

            if(((PlaylistModel)Session["currentplaylist"]).Pages.Count<pageNo)
            {
                return Json(((PlaylistModel)Session["currentplaylist"]).Pages[0], JsonRequestBehavior.AllowGet);
            }

            var item = ((PlaylistModel)Session["currentplaylist"]).Pages[pageNo].ResultRows.FirstOrDefault(r => r.FilePath == playlistItem);
            if (item != null)
            {
                item.Included = onOff;
            }
            return Json(((PlaylistModel)Session["currentplaylist"]).Pages[pageNo], JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreatePlaylist()
        {
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = "playlist.zip",

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            if(Session["currentplaylist"]==null)
                throw new Exception("no playlist on the server.");
            Response.AppendHeader("Content-Disposition", cd.ToString());
            var content = SciendoConfiguration.Container.Resolve<IPlaylistProvider>(
                SciendoConfiguration.PlaylistConfiguration.CurrentPlaylistProvider)
                .CreatePlaylistPackage((PlaylistModel) Session["currentplaylist"]);
            return File(content,"application/zip, application/octet-stream");
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
                        .GetResultsPackageWithVagueStrategy(criteria,numRows,startRow, RequestType.Get), JsonRequestBehavior.AllowGet);
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
                        .GeFilteredtResultsPackageWithVagueStrategy(criteria, numRows, startRow, facetFieldName, facetFieldValue,RequestType.Get),
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
