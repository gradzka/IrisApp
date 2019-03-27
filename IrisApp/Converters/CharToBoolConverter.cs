namespace IrisApp.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class CharToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return false;
            }
            else
            {
                return (char)value == 'L' ? false : true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return 'L';
            }

            return (bool)value == true ? 'R' : 'L';
        }
    }
}
