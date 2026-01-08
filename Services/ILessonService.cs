using EnglishCenter.DTO;
using EnglishCenter.Models;

namespace EnglishCenter.Services
{
    public interface ILessonService
    {
        Task<IEnumerable<Lesson>> GetLessonsAsync(int sectionId);
        Task<Lesson> GetLessonAsync(int id);
        Task<Lesson> AddLessonAsync(LessonCreateDto lesson);
        Task<Lesson> UpdateLessonAsync(int id, LessonUpdateDto lesson);
        Task DeleteLessonAsync(int id);
    }
}
