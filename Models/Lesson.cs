using System.Text.Json.Serialization;

namespace EnglishCenter.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? videoUrl { get; set; }
        public string? videoTitle { get; set; }
        public string? fileUrl { get; set; }
        public string? fileTitle { get; set; }
        public string? imageUrl { get; set; }

        public int Order { get; set; }

        public int SectionId { get; set; }
        [JsonIgnore]
        public Section Section { get; set; }
    }
}
