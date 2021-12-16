using System.ComponentModel.DataAnnotations;

namespace F2021A6MH.Models
{
    public class TrackAddFormViewModel : TrackAddViewModel
    {
        [Required]
        [Display(Name = "Sample clip")]
        [DataType(DataType.Upload)]
        public new string AudioUpload { get; set; }
    }
}