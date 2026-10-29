using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SafeNotePad.Views.Converters
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        #region Constants

        public const string Not = "not";

        public const string Null = "null";

        #endregion

        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (value is bool)
            {
                result = (bool)value;
            }
            else if (value is bool?)
            {
                result = (bool?)value ?? false;
            }

            // 'not' as parameter
            if (null != parameter && string.Equals(Not, parameter as string, StringComparison.OrdinalIgnoreCase))
            {
                return result ? Visibility.Collapsed : Visibility.Visible;
            }

            // 'null' as param
            if (null != parameter && string.Equals(Null, parameter as string, StringComparison.OrdinalIgnoreCase))
            {
                return null == value ? Visibility.Collapsed : Visibility.Visible;
            }

            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is Visibility) && ((Visibility)value == Visibility.Visible);
        }

        #endregion
    }
}
