using System.Windows;
using AkademikAsistan.App.ViewModels;

namespace AkademikAsistan.App.Views.Shell
{
    /// <summary>
    /// Code-behind: sadece constructor + InitializeComponent().
    /// MainViewModel DI container'dan constructor injection ile gelir.
    /// Hiçbir navigasyon veya iş mantığı yoktur.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}
