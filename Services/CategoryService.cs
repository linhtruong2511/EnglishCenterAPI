using EnglishCenter.Data;
using EnglishCenter.Model;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Services
{
    public class CategoryService : ICategoryService
    {
        private DataContext context;
        public CategoryService(DataContext context) {
            this.context = context;
        }
        async Task<Category> ICategoryService.CreateCategory(Category category)
        {
            try
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task ICategoryService.DeleteCategory(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category is not null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy danh mục có id = {id}");
            }
        }

        async Task<IEnumerable<Category>> ICategoryService.GetCategories(string? name)
        {
            var categories = await context.Categories
                .Where(c => c.Name.Contains(name ?? ""))
                .ToListAsync();
            return categories;
        }

        async Task<Category> ICategoryService.GetCategory(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category is not null) { return category; }
            else throw new KeyNotFoundException($"Không tìm thấy danh mục có id = {id}");
        }


        async Task<Category> ICategoryService.UpdateCategory(int id, Category dto)
        {
            var category = await context.Categories.FindAsync(id);
            if (category is not null) 
            { 
                category.Name = dto.Name;
                context.Update(category);
                await context.SaveChangesAsync();
                return category; 
            }
            else throw new KeyNotFoundException($"Không tìm thấy danh mục có id = {id}");
        }
    }
}
