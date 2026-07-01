using AkademikAsistan.Core.Models;
using AkademikAsistan.Services;
using Xunit;

namespace AkademikAsistan.Tests.Services
{
    public class GradeCalculationServiceTests
    {
        private readonly GradeCalculationService _sut = new();

        [Fact]
        public void Calculate_AgirlikliOrtalama_DogruHesaplanir()
        {
            var components = new List<ExamScore>
            {
                new() { ComponentName = "Vize", Score = 80, WeightPercentage = 40 },
                new() { ComponentName = "Final", Score = 90, WeightPercentage = 60 }
            };

            var result = _sut.Calculate(components, DefaultLetterGradeRanges.Standard);

            Assert.Equal(86, result.WeightedScore, precision: 2);
            Assert.Equal("BA", result.LetterGrade);
        }

        [Fact]
        public void Calculate_AgirlikToplami100Degilse_HataFirlatir()
        {
            var components = new List<ExamScore>
            {
                new() { ComponentName = "Vize", Score = 80, WeightPercentage = 30 },
                new() { ComponentName = "Final", Score = 90, WeightPercentage = 60 }
            };

            Assert.Throws<InvalidOperationException>(
                () => _sut.Calculate(components, DefaultLetterGradeRanges.Standard));
        }
    }
}
