using System.Threading.Tasks;
using EnglishCenter.Model;
using EnglishCenter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnglishCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategoryService categoryService { get; set; }
        public CategoryController(ICategoryService categoryService) 
        {
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IEnumerable<Category>> Get(string? name)
        {
            var result = await categoryService.GetCategories(name);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await categoryService.GetCategory(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string name)
        {
            var category  = new Category { Name = name };
            var response  = await categoryService.CreateCategory(category);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<Category> Put(int id, [FromBody] string name)
        {
            var category  = new Category { Name = name };
            var response = await categoryService.UpdateCategory(id, category);
            return category;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await categoryService.DeleteCategory(id);
        }
    }
}
