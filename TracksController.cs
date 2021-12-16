using F2021A6MH.Models;
using System.Web.Mvc;

namespace F2021A6MH.Controllers
{
    [Authorize]
    public class TracksController : BaseController
    {
        public ActionResult Index()
        {
            return View(manager.TrackGetAll());
        }

        public ActionResult Details(int? id)
        {
            var track = manager.TrackGetById(id);

            if (track == null)
                return HttpNotFound();
            else
                return View(track);
        }

        [Route("clip/{id}")]
        public ActionResult Clip(int? id)
        {
            var trackAudio = manager.TrackAudioGetById(id.GetValueOrDefault());

            if (trackAudio == null)
            {
                return HttpNotFound();
            }
            else
            {
                return File(trackAudio.Audio, trackAudio.AudioContentType ?? "Uknown");
            }
        }

        [Authorize(Roles = "Clerk")]
        public ActionResult Delete(int? id)
        {

            var trackToDelete = manager.TrackGetById(id.GetValueOrDefault());

            if (trackToDelete == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(trackToDelete);
            }

        }

        [HttpPost]
        [Authorize(Roles = "Clerk")]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            try
            {
                manager.TrackDelete(id.GetValueOrDefault());
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Clerk")]
        public ActionResult Edit(int? id)
        {
            var track = manager.TrackGetById(id.GetValueOrDefault());

            if (track == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = manager.mapper.Map<TrackWithDetailViewModel, TrackEditFormViewModel>(track);
                return View(form);
            }
        }

        [Authorize(Roles = "Clerk")]
        [HttpPost]
        public ActionResult Edit(int? id, TrackEditViewModel newTrack)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Edit", new { id = newTrack.Id });
                }
                else if (id.GetValueOrDefault() != newTrack.Id)
                {
                    return RedirectToAction("Index");
                }

                var editedTrack = manager.TrackEdit(newTrack);

                if (editedTrack == null)
                {
                    return RedirectToAction("Edit", new { id = newTrack.Id });
                }
                else
                {
                    return RedirectToAction("Details", new { id = newTrack.Id });
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
