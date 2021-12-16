using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace F2021A6MH.Models
{
    public class AlbumAddViewModel
    {
        public AlbumAddViewModel()
        {
            ReleaseDate = DateTime.Now;
        }

        [Required]
        [StringLength(100)]
        [Display(Name = "Album name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Genre { get; set; }

        [Display(Name = "Primary genre")]
        public SelectList AlbumGenreList { get; set; }

        [Required]
        [Display(Name = "Release date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "URL to album image (covert art)")]
        public string UrlAlbum { get; set; }

        [StringLength(200)]
        public string Coordinator { get; set; }

        public int ArtistId { get; set; }

        public string ArtistName { get; set; }

        public string Background { get; set; }
    }
}