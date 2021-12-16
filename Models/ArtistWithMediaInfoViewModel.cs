using System.Collections.Generic;

namespace F2021A6MH.Models
{
    public class ArtistWithMediaInfoViewModel : ArtistWithDetailViewModel
    {
        public ArtistWithMediaInfoViewModel()
        {
            ArtistMediaItems = new List<ArtistMediaItemBaseViewModel>();
        }

        public IEnumerable<ArtistMediaItemBaseViewModel> ArtistMediaItems { get; set; }
    }
}
