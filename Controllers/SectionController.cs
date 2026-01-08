using System.Threading.Tasks;
using EnglishCenter.DTO;
using EnglishCenter.Models;
using EnglishCenter.Services;
using Microsoft.AspNetCore.Mvc;


namespace EnglishCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        ISectionService sectionService;

        public SectionController(ISectionService sectionService)
        {
            this.sectionService = sectionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Section>>> Get(int? courseId)
        {
            if (!courseId.HasValue) 
            {
                return BadRequest("Yêu cầu phải có id của khóa học");
            }

            var result = await sectionService.GetSectionsAsync(courseId.Value);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Section>> Get(int id)
        {
            return Ok(await sectionService.GetSectionAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<Section>> Post([FromBody] SectionCreateDto dto)
        {
            var section = dto as Section;
            return Ok(await sectionService.AddSectionAsync(section));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Section>> Put(int id, [FromBody] SectionUpdateDto dto)
        {
            var section = await sectionService.UpdateSectionAsync(id, new Section
            {
                Name = dto.Name,
                Description = dto.Description,
                Order = dto.Order
            });
            return Ok(section);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await sectionService.DeleteSectionAsync(id);
        }
    }
}
