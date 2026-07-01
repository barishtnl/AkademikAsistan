using System.Collections.ObjectModel;
using System.Windows.Input;
using AkademikAsistan.App.Common;
using AkademikAsistan.Core.Interfaces;
using AkademikAsistan.Core.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace AkademikAsistan.App.ViewModels.GelisimTakibi
{
    /// <summary>
    /// Öğrenci Gelişim Takibi (GNO/YNO) ekranının ViewModel'i. Grafik verisini
    /// LiveCharts2'nin ISeries/Axis tiplerine çevirmekten başka iş yapmaz;
    /// veri SemesterRepository'den, hesaplama mantığı ileride bir
    /// StudentProgressAnalysisService'e taşınabilir (bkz. mimari dokümanı).
    /// </summary>
    public class StudentProgressViewModel : ViewModelBase
    {
        private readonly ISemesterRepository _semesterRepository;

        public StudentProgressViewModel(ISemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;

            Records = new ObservableCollection<SemesterRecord>();
            GpaSeries = Array.Empty<ISeries>();
            XAxes = Array.Empty<Axis>();
            YAxes = new[]
            {
                new Axis
                {
                    MinLimit = 0,
                    MaxLimit = 4,
                    Name = "Not Ortalaması",
                    NamePaint = new SolidColorPaint(SKColor.Parse("#6B7280")),
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#6B7280"))
                }
            };

            LoadCommand = new AsyncRelayCommand(_ => LoadAsync(), onException: HandleError);
            AddSemesterCommand = new AsyncRelayCommand(_ => AddSemesterAsync(), _ => CanAddSemester(), HandleError);

            LoadCommand.Execute(null);
        }

        public ObservableCollection<SemesterRecord> Records { get; }

        private ISeries[] _gpaSeries;
        public ISeries[] GpaSeries { get => _gpaSeries; private set => SetProperty(ref _gpaSeries, value); }

        private Axis[] _xAxes;
        public Axis[] XAxes { get => _xAxes; private set => SetProperty(ref _xAxes, value); }

        public Axis[] YAxes { get; }

        // ----- Yeni dönem formu -----

        private string _newSemesterName = string.Empty;
        public string NewSemesterName { get => _newSemesterName; set => SetProperty(ref _newSemesterName, value); }

        private double _newSemesterGpa;
        public double NewSemesterGpa { get => _newSemesterGpa; set => SetProperty(ref _newSemesterGpa, value); }

        private double _newCumulativeGpa;
        public double NewCumulativeGpa { get => _newCumulativeGpa; set => SetProperty(ref _newCumulativeGpa, value); }

        private int _newTotalCredits;
        public int NewTotalCredits { get => _newTotalCredits; set => SetProperty(ref _newTotalCredits, value); }

        private string? _errorMessage;
        public string? ErrorMessage { get => _errorMessage; private set => SetProperty(ref _errorMessage, value); }

        public ICommand LoadCommand { get; }
        public ICommand AddSemesterCommand { get; }

        private bool CanAddSemester() => !string.IsNullOrWhiteSpace(NewSemesterName);

        private async Task LoadAsync()
        {
            ErrorMessage = null;
            var records = await _semesterRepository.GetAllAsync();

            Records.Clear();
            foreach (var record in records)
                Records.Add(record);

            BuildChart();
        }

        private async Task AddSemesterAsync()
        {
            ErrorMessage = null;

            var record = new SemesterRecord
            {
                SemesterName = NewSemesterName,
                SemesterGpa = NewSemesterGpa,
                CumulativeGpa = NewCumulativeGpa,
                TotalCredits = NewTotalCredits
            };

            record.SemesterRecordId = await _semesterRepository.AddAsync(record);
            Records.Add(record);
            BuildChart();

            NewSemesterName = string.Empty;
            NewSemesterGpa = 0;
            NewCumulativeGpa = 0;
            NewTotalCredits = 0;
        }

        private void BuildChart()
        {
            var list = Records.ToList();

            GpaSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Name = "GNO (Genel)",
                    Values = list.Select(r => r.CumulativeGpa).ToArray(),
                    Stroke = new SolidColorPaint(SKColor.Parse("#E07856"), 3),
                    Fill = null,
                    GeometrySize = 8,
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#E07856"), 3),
                    GeometryFill = new SolidColorPaint(SKColors.White)
                },
                new LineSeries<double>
                {
                    Name = "DNO (Dönemlik)",
                    Values = list.Select(r => r.SemesterGpa).ToArray(),
                    Stroke = new SolidColorPaint(SKColor.Parse("#5B8C72"), 2),
                    Fill = null,
                    GeometrySize = 6,
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#5B8C72"), 2),
                    GeometryFill = new SolidColorPaint(SKColors.White)
                }
            };

            XAxes = new[]
            {
                new Axis
                {
                    Labels = list.Select(r => r.SemesterName).ToArray(),
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#6B7280")),
                    LabelsRotation = 15
                }
            };
        }

        private void HandleError(Exception ex) => ErrorMessage = ex.Message;
    }
}
