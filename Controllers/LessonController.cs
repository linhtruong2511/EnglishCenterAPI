using System.Threading.Tasks;
using EnglishCenter.DTO;
using EnglishCenter.Models;
using EnglishCenter.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        ILessonService lessonService;
        public LessonController(ILessonService lessonService)
        {
            this.lessonService = lessonService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetAll(int sectionId)
        {
            var lessons = await lessonService.GetLessonsAsync(sectionId);
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> Get(int id)
        {
            return await lessonService.GetLessonAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Lesson>> Post([FromForm] LessonCreateDto dto)
        {
            return Ok(await lessonService.AddLessonAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Lesson>> Put(int id, [FromForm] LessonUpdateDto dto)
        {
            return Ok(await lessonService.UpdateLessonAsync(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await lessonService.DeleteLessonAsync(id);
        }
    }
}
