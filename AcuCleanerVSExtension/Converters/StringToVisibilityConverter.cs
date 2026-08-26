using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AcuCleanerVSExtension.Converters
{
    /// <summary>
    /// Converts a string to <see cref="Visibility.Visible"/> when it is non-empty, otherwise <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
