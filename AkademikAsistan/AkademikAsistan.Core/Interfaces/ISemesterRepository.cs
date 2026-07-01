using AkademikAsistan.Core.Models;

namespace AkademikAsistan.Core.Interfaces
{
    /// <summary>
    /// Öğrenci Gelişim Takibi modülünün veri erişim sözleşmesi.
    /// </summary>
    public interface ISemesterRepository
    {
        Task<IReadOnlyList<SemesterRecord>> GetAllAsync();
        Task<int> AddAsync(SemesterRecord record);
    }
}
