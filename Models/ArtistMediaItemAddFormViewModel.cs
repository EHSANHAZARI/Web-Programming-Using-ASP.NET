using System.ComponentModel.DataAnnotations;

namespace F2021A6MH.Models
{
    public class ArtistMediaItemAddFormViewModel : ArtistMediaItemAddViewModel
    {

        [Required]
        [Display(Name = "Media upload")]
        [DataType(DataType.Upload)]
        public new string MediaUpload { get; set; }
    }
}