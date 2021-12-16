using F2021A6MH.EntityModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace F2021A6MH.Models
{
    public class TrackAddViewModel
    {
        public TrackAddViewModel()
        {
            Albums = new List<Album>();
        }

        [StringLength(200)]
        public string Clerk { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Composer name (comma-separated)")]
        public string Composers { get; set; }

        [StringLength(100)]
        public string Genre { get; set; }

        [Display(Name = "Track genre")]
        public SelectList TrackGenreList { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Track name")]
        public string Name { get; set; }

        public int AlbumId { get; set; }

        public string AlbumName { get; set; }

        public IEnumerable<Album> Albums { get; set; }

        [Required]
        [Display(Name = "Sample clip")]
        public HttpPostedFileBase AudioUpload { get; set; }
    }
}