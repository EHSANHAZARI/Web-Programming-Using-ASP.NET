using System;
using System.ComponentModel.DataAnnotations;

namespace F2021A6MH.Models
{
    public class AlbumBaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Album name")]
        public string Name { get; set; }

        [Display(Name = "Album's primary genre")]
        public string Genre { get; set; }

        [Display(Name = "Release date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Album image (cover art)")]
        public string UrlAlbum { get; set; }

        [Display(Name = "Coordinator who looks after the album")]
        public string Coordinator { get; set; }

        [Display(Name = "Album background")]
        public string Background { get; set; }
    }
}