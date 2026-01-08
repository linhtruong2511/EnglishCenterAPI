using System.ComponentModel.DataAnnotations;
using EnglishCenter.Models;

namespace EnglishCenter.Model
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public int Price { get; set; }
        public int Sale { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}
