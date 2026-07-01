using System.Windows.Input;

namespace AkademikAsistan.App.Common
{
    /// <summary>
    /// ICommand'in standart, parametre destekli implementasyonu.
    /// CommandManager.RequerySuggested sayesinde CanExecute, WPF'in komut
    /// sistemi tarafından otomatik olarak (örn. focus değişiminde) yeniden sorgulanır.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>UI'da bir aksiyon sonrası CanExecute'u manuel tetiklemek için.</summary>
        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}
