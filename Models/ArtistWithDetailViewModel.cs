using F2021A6MH.EntityModels;
using System;
using System.Collections.Generic;

namespace F2021A6MH.Models
{
    public class ArtistWithDetailViewModel : ArtistBaseViewModel
    {

        public ArtistWithDetailViewModel()
        {
            Albums = new List<Album>();
            AlbumNames = new List<string>();
            BirthOrStartDate = DateTime.Now;
        }

        public IEnumerable<Album> Albums { get; set; }

        public IEnumerable<string> AlbumNames { get; set; }
    }
}