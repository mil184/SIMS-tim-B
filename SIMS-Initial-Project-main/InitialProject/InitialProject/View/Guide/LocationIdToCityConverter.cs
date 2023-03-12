using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InitialProject.View.Guide
{
    public class LocationIdToCityConverter : IValueConverter
    {
        private readonly LocationRepository _locationRepository = new LocationRepository();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int locationId = (int)value;
            Location location = _locationRepository.GetById(locationId);
            return location.City;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
