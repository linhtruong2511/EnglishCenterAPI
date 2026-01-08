using System.Text.Json.Serialization;
using EnglishCenter.Model;

namespace EnglishCenter.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int CourseId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
