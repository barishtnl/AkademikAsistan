using AkademikAsistan.Core.Models;

namespace AkademikAsistan.Core.Interfaces
{
    /// <summary>
    /// Ders Planı modülünün veri erişim sözleşmesi. ViewModel sadece bu interface'i bilir;
    /// somut implementasyon (Dapper + SQLite) Data katmanındadır.
    /// </summary>
    public interface ICourseRepository
    {
        Task<IReadOnlyList<Course>> GetBySemesterAsync(string semester);
        Task<int> AddAsync(Course course);
        Task DeleteAsync(int courseId);
    }
}
