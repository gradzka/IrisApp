namespace IrisApp.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class SelectedPageBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() != parameter.ToString())
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#999999");
            }
            else
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom("#FFEEEEEE");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
