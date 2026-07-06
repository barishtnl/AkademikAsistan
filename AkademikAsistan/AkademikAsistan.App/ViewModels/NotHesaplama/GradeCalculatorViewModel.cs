using System.Windows.Input;
using AkademikAsistan.App.Common;
using AkademikAsistan.Core.Models;
using AkademikAsistan.Services;

namespace AkademikAsistan.App.ViewModels.NotHesaplama
{
    /// <summary>
    /// Not Hesaplama ekranının ViewModel'i. Hiçbir hesaplama mantığı içermez;
    /// tüm hesaplama IGradeCalculationService'e devredilir. Bu sınıfın tek
    /// sorumluluğu: UI durumunu (property'leri) yönetmek ve servisi çağırmak.
    /// </summary>
    public class GradeCalculatorViewModel : ViewModelBase
    {
        private readonly IGradeCalculationService _gradeCalculationService;

        public GradeCalculatorViewModel(IGradeCalculationService gradeCalculationService)
        {
            _gradeCalculationService = gradeCalculationService
                ?? throw new ArgumentNullException(nameof(gradeCalculationService));

            CalculateCommand = new RelayCommand(_ => Calculate(), _ => CanCalculate());
            ResetCommand = new RelayCommand(_ => Reset());
        }

        // ----- Girdi Property'leri -----

        private double _midtermScore;
        public double MidtermScore
        {
            get => _midtermScore;
            set
            {
                if (SetProperty(ref _midtermScore, value))
                    ClearResult();
            }
        }

        private double _midtermWeight = 40;
        public double MidtermWeight
        {
            get => _midtermWeight;
            set
            {
                if (SetProperty(ref _midtermWeight, value))
                    ClearResult();
            }
        }

        private double _finalScore;
        public double FinalScore
        {
            get => _finalScore;
            set
            {
                if (SetProperty(ref _finalScore, value))
                    ClearResult();
            }
        }

        private double _finalWeight = 60;
        public double FinalWeight
        {
            get => _finalWeight;
            set
            {
                if (SetProperty(ref _finalWeight, value))
                    ClearResult();
            }
        }

        // ----- Sonuç Property'leri (salt okunur; sadece Calculate() tarafından set edilir) -----

        private double _resultWeightedScore;
        public double ResultWeightedScore
        {
            get => _resultWeightedScore;
            private set => SetProperty(ref _resultWeightedScore, value);
        }

        private string _resultLetterGrade = string.Empty;
        public string ResultLetterGrade
        {
            get => _resultLetterGrade;
            private set => SetProperty(ref _resultLetterGrade, value);
        }

        private double _resultGradePoint;
        public double ResultGradePoint
        {
            get => _resultGradePoint;
            private set => SetProperty(ref _resultGradePoint, value);
        }

        private bool _hasResult;
        public bool HasResult
        {
            get => _hasResult;
            private set => SetProperty(ref _hasResult, value);
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            private set => SetProperty(ref _errorMessage, value);
        }

        // ----- Komutlar -----

        public ICommand CalculateCommand { get; }
        public ICommand ResetCommand { get; }

        private bool CanCalculate() =>
            MidtermScore is >= 0 and <= 100 &&
            FinalScore is >= 0 and <= 100;

        private void Calculate()
        {
            ErrorMessage = null;

            try
            {
                var components = new List<ExamScore>
                {
                    new() { ComponentName = "Vize",  Score = MidtermScore, WeightPercentage = MidtermWeight },
                    new() { ComponentName = "Final", Score = FinalScore,   WeightPercentage = FinalWeight }
                };

                GradeCalculationResult result = _gradeCalculationService.Calculate(
                    components,
                    DefaultLetterGradeRanges.Standard);

                ResultWeightedScore = result.WeightedScore;
                ResultLetterGrade = result.LetterGrade;
                ResultGradePoint = result.GradePoint;
                HasResult = true;
            }
            catch (Exception ex)
            {
                // Servis katmanından gelen iş kuralı hatalarını (ör. ağırlık toplamı %100 değil)
                // doğrudan kullanıcıya gösterilebilir mesaja çeviriyoruz.
                ErrorMessage = ex.Message;
                HasResult = false;
            }
        }

        private void Reset()
        {
            MidtermScore = 0;
            FinalScore = 0;
            MidtermWeight = 40;
            FinalWeight = 60;
            ClearResult();
        }

        private void ClearResult()
        {
            HasResult = false;
            ErrorMessage = null;
        }
    }
}
