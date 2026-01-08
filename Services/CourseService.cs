using EnglishCenter.Data;
using EnglishCenter.Extensions;
using EnglishCenter.Model;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Services
{
    public class CourseService : ICourseService
    {
        private DataContext context;
        private IFileService fileService;
        public CourseService(DataContext context,
            IWebHostEnvironment env,
            IFileService file)
        {
            this.fileService = file;
            this.context = context;
        }

        async Task<Course> ICourseService.AddCourse(Course course)
        {
            await context.Courses.AddAsync(course);
            context.SaveChanges();
            return course;
        }

        async Task ICourseService.DeleteCourse(int id)
        {
            var course = await context.Courses.FindAsync(id);
            if (course is not null)
            {
                context.Courses.Remove(course);
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Không tìm thấy khóa học có id hợp lệ");
            }
        }

        async Task<Course> ICourseService.GetCourseById(int id)
        {
            var course = await context.Courses
                .Include(c => c.Sections.OrderBy(s => s.Order))
                    .ThenInclude(s => s.Lessons.OrderBy(l => l.Order))
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course is not null)
            {
                return course;
            }
            else
            {
                throw new ArgumentException($"Không tìm thấy khóa học có id hợp lệ");
            }
        }

        async Task<IEnumerable<Course>> ICourseService.GetCourses(string? name)
        {
            var courses = await context.Courses
                .Where(c => c.Name.Contains(name ?? ""))
                .Include(c => c.Category)
                .Include(c => c.Tags)
                .OrderByDescending(c => c.Name)
                .ToListAsync();

            return courses;
        }

        async Task<Course> ICourseService.UpdateCourse(int id, Course data)
        {
            var course = await context.Courses.FindAsync(id);
            if (course is not null)
            {
                course.Name = data.Name;
                course.Description = data.Description;
                context.Update(course); 
                context.SaveChanges();

                return course;
            }
            else
            {
                throw new ArgumentException($"Không tìm thấy khóa học có id hợp lệ");
            }
        }

        async Task<Course> ICourseService.UploadImage(int id, IFormFile file)
        {
            var course = await context.Courses.FindAsync(id);

            if (course is not null)
            {
                //var imageUrl = await file.SaveImageAsync("course");
                var imageUrl = await fileService.SaveImageAsync(file, "images/courses");
                course.ImageUrl = imageUrl;
                context.Update(course);
                context.SaveChanges();
                return course;
            } 
            else
            {
                throw new KeyNotFoundException("id khóa học không hợp lệ");
            }
        }
    }
}
