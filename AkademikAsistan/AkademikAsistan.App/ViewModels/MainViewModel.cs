using System.Windows.Input;
using AkademikAsistan.App.Common;
using AkademikAsistan.App.ViewModels.DersPlani;
using AkademikAsistan.App.ViewModels.GelisimTakibi;
using AkademikAsistan.App.ViewModels.NotHesaplama;

namespace AkademikAsistan.App.ViewModels
{
    /// <summary>
    /// Ana pencerenin (kabuk) ViewModel'i. Sol menüden hangi modülün seçildiğini
    /// tutar; View tarafında CurrentViewModel'e bağlı bir ContentControl,
    /// App.xaml'deki DataTemplate eşlemeleri sayesinde doğru View'i otomatik gösterir
    /// (ViewModel-first navigasyon — code-behind'da hiçbir View değiştirme kodu yoktur).
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(
            GradeCalculatorViewModel gradeCalculatorViewModel,
            DersPlaniViewModel dersPlaniViewModel,
            StudentProgressViewModel studentProgressViewModel)
        {
            GradeCalculatorViewModel = gradeCalculatorViewModel;
            DersPlaniViewModel = dersPlaniViewModel;
            StudentProgressViewModel = studentProgressViewModel;

            NavigateCommand = new RelayCommand(Navigate);
            CurrentViewModel = dersPlaniViewModel;
            ActiveModule = "DersPlani";
        }

        public DersPlaniViewModel DersPlaniViewModel { get; }
        public GradeCalculatorViewModel GradeCalculatorViewModel { get; }
        public StudentProgressViewModel StudentProgressViewModel { get; }

        private object _currentViewModel = null!;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            private set => SetProperty(ref _currentViewModel, value);
        }

        private string _activeModule = string.Empty;
        public string ActiveModule
        {
            get => _activeModule;
            private set => SetProperty(ref _activeModule, value);
        }

        public ICommand NavigateCommand { get; }

        private void Navigate(object? moduleKey)
        {
            switch (moduleKey as string)
            {
                case "DersPlani":
                    CurrentViewModel = DersPlaniViewModel;
                    ActiveModule = "DersPlani";
                    break;
                case "NotHesaplama":
                    CurrentViewModel = GradeCalculatorViewModel;
                    ActiveModule = "NotHesaplama";
                    break;
                case "GelisimTakibi":
                    CurrentViewModel = StudentProgressViewModel;
                    ActiveModule = "GelisimTakibi";
                    break;
            }
        }
    }
}
