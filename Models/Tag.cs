using System.Text.Json.Serialization;

namespace EnglishCenter.Model
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Course> Courses { get; set; }
    }
}
