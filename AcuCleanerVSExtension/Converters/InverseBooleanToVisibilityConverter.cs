using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AcuCleanerVSExtension.Converters
{
    /// <summary>
    /// Converts a boolean to <see cref="Visibility.Visible"/> when it is <c>false</c>, otherwise <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool boolValue && !boolValue ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
