using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class LocationRepository
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";
        
        private readonly Serializer<Location> _serializer;
        
        private List<Location> _locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            _locations = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _locations = _serializer.FromCSV(FilePath);
            if (_locations.Count < 1)
            {
                return 1;
            }
            return _locations.Max(c => c.Id) + 1;
        }

        public Location Save(Location location)
        {
            location.Id = NextId();

            _locations = _serializer.FromCSV(FilePath);
            _locations.Add(location);
            _serializer.ToCSV(FilePath, _locations);

            return location;
        }
        public Location GetById(int id)
        {
            _locations = _serializer.FromCSV(FilePath);
            return _locations.Find(c => c.Id == id);
        }
    }
}
