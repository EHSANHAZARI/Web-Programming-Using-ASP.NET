using F2021A6MH.EntityModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace F2021A6MH.Models
{
    public class TrackWithDetailViewModel : TrackBaseViewModel
    {
        public TrackWithDetailViewModel()
        {
            Albums = new List<Album>();
            AlbumNames = new List<string>();
        }

        [Display(Name = "Albums with this track")]
        public IEnumerable<string> AlbumNames { get; set; }

        public IEnumerable<Album> Albums { get; set; }
    }
}