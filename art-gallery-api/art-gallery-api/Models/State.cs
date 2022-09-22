using System;
namespace art_gallery_api.Models
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int LanguageGroups { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public DateTime ModifiedDate { get; set; }
    }
}

