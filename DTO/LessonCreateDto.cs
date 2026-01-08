using System.ComponentModel.DataAnnotations;

namespace EnglishCenter.DTO
{
    public class LessonCreateDto 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Video { get; set; }
        public IFormFile? File {  get; set; }
        public IFormFile? Image { get; set; }
        public int? Order { get; set; }
        [Required(ErrorMessage = "Id section là bắt buộc")]
        public int SectionId { get; set; }
    }
}
