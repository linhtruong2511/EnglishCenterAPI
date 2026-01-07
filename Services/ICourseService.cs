using EnglishCenter.Model;

namespace EnglishCenter.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetCourses(string? name);
        Task<Course> GetCourseById(int id);
        Task<Course> AddCourse(Course course);
        Task<Course> UpdateCourse(int id, Course course);
        Task DeleteCourse(int id);
        Task<Course> UploadImage(int id, IFormFile file);
    }
}
