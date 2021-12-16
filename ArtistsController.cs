using F2021A6MH.Models;
using System.Web.Mvc;

namespace F2021A6MH.Controllers
{
    [Authorize]
    public class ArtistsController : BaseController
    {
        public ActionResult Index()
        {
            var artists = manager.ArtistGetAll();
            return View(artists);
        }

        public ActionResult Details(int? id)
        {
            var artist = manager.ArtistWithMediaInformationGetById(id);

            if (artist == null)
                return HttpNotFound();
            else
                return View(artist);
        }

        [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            var artist = new ArtistAddFormViewModel()
            {
                Executive = User.Identity.Name,
                ArtistGenreList = GetGenreSelectList()
            };

            return View(artist);
        }

        [Authorize(Roles = "Executive")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ArtistAddViewModel artist)
        {
            try
            {
                if (!ModelState.IsValid) return View(artist);

                var addedArtist = manager.ArtistAdd(artist);

                if (addedArtist == null)
                    return View(addedArtist);
                else
                    return RedirectToAction(nameof(Details), new { id = addedArtist.Id });
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Coordinator")]
        [Route("Artist/{id}/AddAlbum")]
        public ActionResult AddAlbum(int? id)
        {
            var artist = manager.ArtistGetById(id);

            if (artist == null) return HttpNotFound();

            var albumAdd = new AlbumAddFormViewModel()
            {
                ArtistId = artist.Id,
                ArtistName = artist.Name,
                AlbumGenreList = GetGenreSelectList()
            };

            return View(albumAdd);
        }

        [Authorize(Roles = "Coordinator")]
        [Route("Artist/{id}/Addalbum")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddAlbum(AlbumAddViewModel album)
        {
            try
            {
                if (!ModelState.IsValid) return View(album);

                var addedAlbum = manager.AlbumAdd(album);

                if (addedAlbum == null)
                    return View(album);
                else
                    return RedirectToAction(nameof(Details), "Albums", new { id = addedAlbum.Id });
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Coordinator")]
        [Route("artists/{id}/addmediaitem")]
        public ActionResult AddMediaItem(int? id)
        {
            var artist = manager.ArtistGetById(id.GetValueOrDefault());

            if (artist == null)
            {
                return HttpNotFound();
            }
            else
            {
                var mediaForm = new ArtistMediaItemAddFormViewModel();

                mediaForm.ArtistId = artist.Id;
                mediaForm.ArtistName = artist.Name;

                return View(mediaForm);
            }

        }

        [HttpPost]
        [Authorize(Roles = "Coordinator")]
        [Route("artists/{id}/addmediaitem")]
        public ActionResult AddMediaItem(int? id, ArtistMediaItemAddViewModel newMedia)
        {
            try
            {
                if (!ModelState.IsValid && id.GetValueOrDefault() == newMedia.ArtistId)
                {
                    return View(newMedia);
                }

                var artistWithMedia = manager.ArtistMediaAdd(newMedia);

                if (artistWithMedia == null)
                {
                    return View(newMedia);
                }
                else
                {
                    return RedirectToAction("details", "artists", new { id = artistWithMedia.Id });
                }

            }
            catch
            {
                return View();
            }

        }
    }
}
