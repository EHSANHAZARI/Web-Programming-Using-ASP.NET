using System.ComponentModel.DataAnnotations;

namespace F2021A6MH.Models
{
    public class TrackEditFormViewModel : TrackEditViewModel
    {

        public string Name { get; set; }

        [Required]
        [Display(Name = "Sample clip")]
        [DataType(DataType.Upload)]
        public new string AudioUpload { get; set; }
    }
}