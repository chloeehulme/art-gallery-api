using System;
namespace art_gallery_api.Models
{
    public class Artefact
    {
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Medium { get; set; } = null!;
        public int Year { get; set; }
        public double HeightCm { get; set; }
        public double WidthCm { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}

