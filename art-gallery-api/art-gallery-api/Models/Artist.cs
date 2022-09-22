using System;
namespace art_gallery_api.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Age { get; set; }
        public string State { get; set; } = null!;
        public string LanguageGroup { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}

