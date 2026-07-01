using AkademikAsistan.Core.Models;

namespace AkademikAsistan.Services
{
    /// <summary>
    /// Veritabanı henüz hazır değilken veya seed verisi yoksa kullanılabilecek
    /// varsayılan harf notu aralıkları. ÖNEMLİ: Bu değerler örnek/placeholder'dır.
    /// Gerçek uygulamada bu liste LetterGradeRanges tablosundan (Repository üzerinden)
    /// okunmalı; buradaki sabit değerleri kendi konsol uygulamandaki resmi
    /// Gazi Üniversitesi kriterleriyle değiştir.
    /// </summary>
    public static class DefaultLetterGradeRanges
    {
        public static IReadOnlyList<LetterGradeRange> Standard { get; } = new List<LetterGradeRange>
        {
            new() { LetterGrade = "AA", MinScore = 90, MaxScore = 100,    GradePoint = 4.00 },
            new() { LetterGrade = "BA", MinScore = 85, MaxScore = 89.99,  GradePoint = 3.50 },
            new() { LetterGrade = "BB", MinScore = 80, MaxScore = 84.99,  GradePoint = 3.00 },
            new() { LetterGrade = "CB", MinScore = 75, MaxScore = 79.99,  GradePoint = 2.50 },
            new() { LetterGrade = "CC", MinScore = 70, MaxScore = 74.99,  GradePoint = 2.00 },
            new() { LetterGrade = "DC", MinScore = 65, MaxScore = 69.99,  GradePoint = 1.50 },
            new() { LetterGrade = "DD", MinScore = 60, MaxScore = 64.99,  GradePoint = 1.00 },
            new() { LetterGrade = "FD", MinScore = 50, MaxScore = 59.99,  GradePoint = 0.50 },
            new() { LetterGrade = "FF", MinScore = 0,  MaxScore = 49.99,  GradePoint = 0.00 },
        };
    }
}
