using EnglishCenter.Data;
using EnglishCenter.DTO;
using EnglishCenter.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Services
{
    public class LessonService : ILessonService
    {
        private readonly DataContext _context;
        private IFileService fileService;

        public LessonService(
            DataContext context,
            IFileService fileService)
        {
            _context = context;
            this.fileService = fileService;
        }

        public async Task<IEnumerable<Lesson>> GetLessonsAsync(int sectionId)
        {
            return await _context.Lessons
                .Where(l => l.SectionId == sectionId)
                .OrderBy(l => l.Order)
                .ToListAsync();
        }

        public async Task<Lesson> GetLessonAsync(int id)
        {
            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
                throw new KeyNotFoundException($"Lesson {id} not found");

            return lesson;
        }

        public async Task<Lesson> AddLessonAsync(LessonCreateDto dto)
        {
            var lesson = new Lesson
            {
                SectionId = dto.SectionId,
                Title = dto.Title,
                Description = dto.Description,
                Order = dto.Order ?? _context.Sections.Count() + 1,
            };

            if (dto.File != null)
            {
                var fileUrl = await fileService.SaveFileAsync(dto.File, "/files/lessons"); 
                lesson.fileUrl = fileUrl;
            }
            if (dto.Image != null)
            {
                var imageUrl = await fileService.SaveImageAsync(dto.Image, "/images/lessons"); 
                lesson.imageUrl = imageUrl;
            }
            if (dto.Video != null)
            {
                var videoUrl = await fileService.SaveVideoAsync(dto.Video, "/videos/lessons"); 
                lesson.videoUrl = videoUrl;
            }
            
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Lesson> UpdateLessonAsync(int id, LessonUpdateDto dto)
        {
            var existing = await _context.Lessons.FindAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Lesson {id} not found");

            existing.Title = dto.Title ?? existing.Title;
            existing.Description = dto.Description ?? existing.Description;
            existing.Order = dto.Order ?? existing.Order;

            if (dto.File != null)
            {
                var fileUrl = await fileService.SaveFileAsync(dto.File, "files/lessons");
                existing.fileUrl = fileUrl;
            }
            if (dto.Image != null)
            {
                var imageUrl = await fileService.SaveImageAsync(dto.Image, "images/lessons");
                existing.imageUrl = imageUrl;
            }
            if (dto.Video != null)
            {
                var videoUrl = await fileService.SaveVideoAsync(dto.Video, "videos/lessons");
                existing.videoUrl = videoUrl;
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
                throw new KeyNotFoundException($"Lesson {id} not found");

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
        }
    }
}
