using System.ComponentModel.DataAnnotations;
using System.Web;

namespace F2021A6MH.Models
{
    public class TrackEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public HttpPostedFileBase AudioUpload { get; set; }
    }
}