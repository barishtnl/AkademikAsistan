using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AkademikAsistan.App.Converters
{
    /// <summary>bool → Visibility. true ise Visible, false ise Collapsed.</summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// String null/boş değilse Visible, aksi hâlde Collapsed döner.
    /// Hata mesajı TextBlock'larının görünürlüğünü kontrol eder.
    /// </summary>
    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => string.IsNullOrWhiteSpace(value as string) ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// value.ToString() == parameter.ToString() ise true döner.
    /// Sidebar RadioButton'larının IsChecked durumunu yönetmek için kullanılır.
    /// Örn: ActiveModule="DersPlani", ConverterParameter="DersPlani" → true (seçili).
    /// </summary>
    public class StringEqualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value?.ToString() == parameter?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
