using F2021A6MH.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace F2021A6MH.Models
{
    public class ArtistAddViewModel
    {
        public ArtistAddViewModel()
        {
            BirthOrStartDate = DateTime.Now;
            Albums = new List<Album>();
        }

        [Required]
        [StringLength(100)]
        [Display(Name = "Artist name or stage name")]
        public string Name { get; set; }

        [StringLength(100)]
        [Display(Name = "If applicable, artist birth name")]
        public string BirthName { get; set; }

        [Required]
        [Display(Name = "Birth date, or start date")]
        [DataType(DataType.Date)]
        public DateTime BirthOrStartDate { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "Artist Photo")]
        public string UrlArtist { get; set; }

        public string Genre { get; set; }

        [Display(Name = "Artist's primary genre")]
        public SelectList ArtistGenreList { get; set; }

        [StringLength(200)]
        [Display(Name = "Executive who looks after this artist")]
        public string Executive { get; set; }

        public string Career { get; set; }

        public IEnumerable<Album> Albums { get; set; }
    }
}