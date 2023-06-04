using System;
using System.Globalization;
using System.Windows.Data;

namespace InitialProject.Converters
{
    public class RatingRadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int rating = (int)value;
            int radioButtonValue = int.Parse(parameter.ToString());
            return rating == radioButtonValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (isChecked)
                return int.Parse(parameter.ToString());
            return Binding.DoNothing;
        }
    }
}