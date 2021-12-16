using F2021A6MH.Models;
using System.Web.Mvc;

namespace F2021A6MH.Controllers
{
    [Authorize]
    public class AlbumsController : BaseController
    {
        public ActionResult Index()
        {
            var albums = manager.AlbumGetAll();
            return View(albums);
        }

        public ActionResult Details(int? id)
        {
            var album = manager.AlbumGetById(id);

            if (album == null)
                return HttpNotFound();
            else return View(album);
        }

        [Route("Album/{id}/AddTrack")]
        [Authorize(Roles = "Clerk")]
        public ActionResult AddTrack(int? id)
        {
            var album = manager.AlbumGetById(id);

            if (album == null) return HttpNotFound();

            var trackAdd = new TrackAddFormViewModel
            {
                AlbumId = album.Id,
                AlbumName = album.Name,
                TrackGenreList = GetGenreSelectList()
            };

            return View(trackAdd);
        }

        [Route("Album/{id}/AddTrack")]
        [HttpPost]
        [Authorize(Roles = "Clerk")]
        public ActionResult AddTrack(TrackAddViewModel track)
        {
            try
            {
                if (!ModelState.IsValid) return View(track);

                var addedTrack = manager.TrackAdd(track);

                if (addedTrack == null)
                    return View(track);
                else
                    return RedirectToAction(nameof(Details), "tracks", new { id = addedTrack.Id });
            }
            catch
            {
                return View();
            }
        }
    }

}
