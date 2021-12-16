using System.ComponentModel.DataAnnotations;

namespace F2021A6MH.Models
{
    public class TrackBaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Clerk who helps with album tasks")]
        public string Clerk { get; set; }

        [Display(Name = "Composer name(s)")]
        public string Composers { get; set; }

        [Display(Name = "Track genre")]
        public string Genre { get; set; }

        [Display(Name = "Track name")]
        public string Name { get; set; }
    }
}