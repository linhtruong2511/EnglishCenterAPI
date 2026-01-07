using System.Threading.Tasks;
using EnglishCenter.DTO;
using EnglishCenter.Model;
using EnglishCenter.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnglishCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        ICourseService courseService;
        public CourseController(ICourseService courseService) 
        {
            this.courseService = courseService;
        }

        [HttpGet]
        public async Task<IEnumerable<Course>> Get(string? name)
        {
            var courses = await courseService.GetCourses(name);
            return courses;
        }

        [HttpGet("{id}")]
        public async Task<Course> Get(int id)
        {
            var course = await courseService.GetCourseById(id);
            return course;
        }

        [HttpPost]
        public async Task<Course> Post([FromBody] CourseCreateDto dto)
        {
            var course = new Course { Name = dto.Name, Description = dto.Description };
            var result = await courseService.AddCourse(course);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<Course> Put(int id, [FromBody] CourseUpdateDto dto)
        {
            var course = new Course { Name = dto.Name, Description = dto.Description };
            var result = await courseService.UpdateCourse(id, course);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await courseService.DeleteCourse(id);
        }

        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadImage(
            int id,
            IFormFile file)
        {
            var course = await courseService.UploadImage(id, file);
            return Ok(course);
        }
    }
}
