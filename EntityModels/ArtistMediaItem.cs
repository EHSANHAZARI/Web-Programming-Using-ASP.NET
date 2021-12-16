using System;
using System.ComponentModel.DataAnnotations;

namespace F2021A6MH.EntityModels
{
    public class ArtistMediaItem
    {
        public ArtistMediaItem()
        {
            Timestamp = DateTime.Now;

            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            StringId = string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string StringId { get; set; }

        [Required, StringLength(100)]
        public string Caption { get; set; }

        public byte[] Content { get; set; }

        [StringLength(200)]
        public string ContentType { get; set; }

        public DateTime Timestamp { get; set; }

        [Required]
        public Artist Artist { get; set; }
    }
}
