using EnglishCenter.Data;
using EnglishCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Services
{
    public class SectionService : ISectionService
    {
        private readonly DataContext _context;

        public SectionService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Section>> GetSectionsAsync(int courseId)
        {
            return await _context.Sections
                .Where(s => s.CourseId == courseId)
                .Include(s => s.Lessons.OrderBy(o => o.Order))
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task<Section> GetSectionAsync(int id)
        {
            var section = await _context.Sections
                .Include(s => s.Lessons)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (section == null)
                throw new KeyNotFoundException($"Section {id} not found");

            return section;
        }

        public async Task<Section> AddSectionAsync(Section section)
        {
            _context.Sections.Add(section);
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task<Section> UpdateSectionAsync(int id, Section section)
        {
            var existing = await _context.Sections.FindAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Section {id} not found");

            existing.Name = section.Name;
            existing.Description = section.Description;
            existing.Order = section.Order;
            existing.CourseId = section.CourseId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteSectionAsync(int id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null)
                throw new KeyNotFoundException($"Section {id} not found");

            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
        }
    }
}
