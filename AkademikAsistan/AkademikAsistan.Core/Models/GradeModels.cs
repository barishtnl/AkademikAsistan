namespace AkademikAsistan.Core.Models
{
    /// <summary>
    /// Bir derse ait tek bir not bileşeni (Vize, Final, Ödev vb.).
    /// UI'dan ve veritabanından tamamen bağımsız, saf domain modeli.
    /// </summary>
    public class ExamScore
    {
        public string ComponentName { get; set; } = string.Empty;
        public double Score { get; set; }
        public double WeightPercentage { get; set; }
    }

    /// <summary>
    /// Bir harf notuna karşılık gelen puan aralığı (örn. AA: 90-100, 4.00).
    /// LetterGradeRanges tablosundan veya sabit bir listeden enjekte edilir.
    /// </summary>
    public class LetterGradeRange
    {
        public string LetterGrade { get; set; } = string.Empty;
        public double MinScore { get; set; }
        public double MaxScore { get; set; }
        public double GradePoint { get; set; }
    }

    /// <summary>
    /// GradeCalculationService'in ürettiği sonuç. ViewModel bu nesneyi
    /// doğrudan property'lerine yansıtır, hiçbir hesaplama yapmaz.
    /// </summary>
    public class GradeCalculationResult
    {
        public double WeightedScore { get; set; }
        public string LetterGrade { get; set; } = string.Empty;
        public double GradePoint { get; set; }
        public bool IsPassed { get; set; }
    }
}
