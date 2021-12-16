using Microsoft.Win32;
using System.Web.Mvc;

namespace F2021A6MH.Controllers
{
    public class ArtistMediaController : BaseController
    {
        [Route("media/{stringId}")]
        public ActionResult Details(string stringId = "")
        {
            var media = manager.MediaGetById(stringId);

            if (media == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (media.ContentType != null)
                {
                    return File(media.Content, media.ContentType);
                }

                return Content("No media");
            }
        }

        [Route("media/{stringId}/download")]
        public ActionResult DetailsDownload(string stringId = "")
        {
            var media = manager.MediaGetById(stringId);

            if (media == null)
            {
                return HttpNotFound();
            }
            else
            {
                RegistryKey key;
                object value;
                string extension;

                //This code only works on local machine. As this code is given code, I will follow professor codes although it is not working on the Azure.
                key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + media.ContentType, false);
                value = (key == null) ? null : key.GetValue("Extension", null);
                extension = (value == null) ? string.Empty : value.ToString();

                //Following code is works both in Azure and local
                //extension = MimeTypeMap.GetExtension(media.ContentType);

                var contentDisposition = new System.Net.Mime.ContentDisposition
                {
                    FileName = $"media-{stringId}{extension}",
                    Inline = false
                };

                Response.AppendHeader("Content-Disposition", contentDisposition.ToString());
                return File(media.Content, media.ContentType);
            }
        }
    }
}
