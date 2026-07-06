using AkademikAsistan.Core.Models;

namespace AkademikAsistan.Services
{
    /// <summary>
    /// Not hesaplama iş mantığının sözleşmesi. ViewModel sadece bu interface'i bilir;
    /// somut implementasyon DI container üzerinden enjekte edilir (DIP).
    /// </summary>
    public interface IGradeCalculationService
    {
        GradeCalculationResult Calculate(IEnumerable<ExamScore> components, IEnumerable<LetterGradeRange> letterGradeRanges);
    }

    /// <summary>
    /// Ağırlıklı ortalama + harf notu eşleme mantığı.
    /// Bağıl (çan eğrisi) sistem gerekirse, bu interface'in başka bir implementasyonu
    /// (örn. CurvedGradeCalculationService) sınıf ortalaması/standart sapma alarak
    /// aynı sözleşmeyi karşılayabilir (Strategy Pattern) — ViewModel değişmeden kalır.
    /// </summary>
    public class GradeCalculationService : IGradeCalculationService
    {
        public GradeCalculationResult Calculate(IEnumerable<ExamScore> components, IEnumerable<LetterGradeRange> letterGradeRanges)
        {
            var componentList = components?.ToList() ?? throw new ArgumentNullException(nameof(components));
            var rangeList = letterGradeRanges?.ToList() ?? throw new ArgumentNullException(nameof(letterGradeRanges));

            if (componentList.Count == 0)
                throw new ArgumentException("En az bir not bileşeni girilmelidir.", nameof(components));

            double totalWeight = componentList.Sum(c => c.WeightPercentage);
            if (Math.Abs(totalWeight - 100.0) > 0.01)
                throw new InvalidOperationException($"Ağırlıkların toplamı %100 olmalıdır (girilen: %{totalWeight}).");

            if (componentList.Any(c => c.Score < 0 || c.Score > 100))
                throw new InvalidOperationException("Notlar 0-100 aralığında olmalıdır.");

            double weightedScore = componentList.Sum(c => c.Score * c.WeightPercentage / 100.0);

            var matchedRange = rangeList
                .FirstOrDefault(r => weightedScore >= r.MinScore && weightedScore <= r.MaxScore);

            if (matchedRange == null)
                throw new InvalidOperationException("Hesaplanan puana karşılık gelen bir harf notu aralığı bulunamadı.");

            return new GradeCalculationResult
            {
                WeightedScore = Math.Round(weightedScore, 2),
                LetterGrade = matchedRange.LetterGrade,
                GradePoint = matchedRange.GradePoint,
                IsPassed = matchedRange.GradePoint >= 2.00 // CC ve üzeri geçer kabul edildi; kendi kriterine göre güncelle
            };
        }
    }
}
