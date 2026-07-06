using System.Windows.Input;

namespace AkademikAsistan.App.Common
{
    /// <summary>
    /// Veritabanı çağrıları gibi async işlemler için ICommand implementasyonu.
    /// Çalışırken tekrar tetiklenmeyi engeller (IsExecuting koruması) ve
    /// hata fırlatmaz; onun yerine OnException ile dışarıya bildirir.
    /// </summary>
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object?, Task> _execute;
        private readonly Predicate<object?>? _canExecute;
        private readonly Action<Exception>? _onException;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<object?, Task> execute, Predicate<object?>? canExecute = null, Action<Exception>? onException = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            _onException = onException;
        }

        public bool CanExecute(object? parameter) => !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);

        public async void Execute(object? parameter)
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();
            try
            {
                await _execute(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}
