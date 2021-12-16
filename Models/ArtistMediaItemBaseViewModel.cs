using System;
using System.ComponentModel.DataAnnotations;

namespace F2021A6MH.Models
{
    public class ArtistMediaItemBaseViewModel
    {

        public ArtistMediaItemBaseViewModel()
        {
            Timestamp = DateTime.Now;

            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= b + 1;
            }
            StringId = string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public int MediaId { get; set; }

        [Required]
        public string StringId { get; set; }

        [Required]
        public string Caption { get; set; }
        public DateTime Timestamp { get; set; }

        public string ContentType { get; set; }
    }
}