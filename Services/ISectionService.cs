using EnglishCenter.Models;

namespace EnglishCenter.Services
{
    public interface ISectionService
    {
        Task<IEnumerable<Section>> GetSectionsAsync(int courseId);
        Task<Section> GetSectionAsync(int id);
        Task<Section> AddSectionAsync(Section section);
        Task<Section> UpdateSectionAsync(int id, Section section);
        Task DeleteSectionAsync(int id);
    }
}
