using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AkademikAsistan.App.Common
{
    /// <summary>
    /// Tüm ViewModel'lerin türediği temel sınıf. INotifyPropertyChanged altyapısını
    /// tek bir yerde toplar; her ViewModel'in kendi içinde tekrar implemente etmesi gerekmez.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Backing field'ı günceller ve değer gerçekten değiştiyse PropertyChanged tetikler.
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
