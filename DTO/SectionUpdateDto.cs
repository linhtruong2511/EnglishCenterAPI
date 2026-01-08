using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.DTO
{
    public class SectionUpdateDto
    {
        [Required]
        public int Order { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
