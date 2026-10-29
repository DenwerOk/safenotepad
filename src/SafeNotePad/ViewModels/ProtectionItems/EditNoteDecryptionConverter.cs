using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SafeNotePad.ViewModels.ProtectionItems
{
    class EditNoteDecryptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var inverse = ((string) parameter).Contains("NOT");
            var param = ((string) parameter).Replace("NOT", string.Empty);
            if (value != null)
            {
                if (((DecryptionType) (int) value).ToString() == param)
                {
                    return !inverse ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    return !inverse ? Visibility.Collapsed : Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
