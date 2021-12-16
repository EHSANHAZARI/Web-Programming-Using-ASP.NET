using System;
using System.ComponentModel.DataAnnotations;

namespace F2021A6MH.Models
{
    public class ArtistBaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Artist name or stage name")]
        public string Name { get; set; }

        [Display(Name = "If applicable, artist's birth name")]
        public string BirthName { get; set; }

        [Display(Name = "Birth date, or start date")]
        [DataType(DataType.Date)]
        public DateTime BirthOrStartDate { get; set; }

        [Display(Name = "Artist Photo")]
        public string UrlArtist { get; set; }

        [Display(Name = "Artist's primary genre")]
        public string Genre { get; set; }

        [Display(Name = "Executive who looks after this artist")]
        public string Executive { get; set; }

        [Display(Name = "Artist career")]
        public string Career { get; set; }
    }
}