using F2021A6MH.EntityModels;
using System;
using System.Collections.Generic;

namespace F2021A6MH.Models
{
    public class AlbumWithDetailViewModel : AlbumBaseViewModel
    {
        public AlbumWithDetailViewModel()
        {
            Artists = new List<Artist>();
            Tracks = new List<Track>();
            ArtistNames = new List<string>();
            ReleaseDate = DateTime.Now;
        }

        public IEnumerable<string> ArtistNames { get; set; }

        public IEnumerable<Artist> Artists { get; set; }

        public IEnumerable<Track> Tracks { get; set; }
    }
}