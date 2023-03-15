using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class LocationRepository : ISubject
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";
        
        private readonly Serializer<Location> _serializer;
        private readonly List<IObserver> _observers;

        private List<Location> _locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            _locations = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
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
            NotifyObservers();

            return location;
        }
        public Location GetById(int id)
        {
            _locations = _serializer.FromCSV(FilePath);
            return _locations.Find(c => c.Id == id);
        }
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }
        public List<string> GetCountries()
        {
            List<string> countries = new List<string>();
            foreach(Location location in _locations) 
            {
                if(!countries.Contains(location.Country))
                countries.Add(location.Country);
            }
            return countries;
        }
        public List<string> GetCities(String country)
        {
            List<string> cities = new List<string>();
            foreach (Location location in _locations)
            {
                if(location.Country == country) 
                {
                    cities.Add(location.City);   
                }
            }
            return cities;
        }
        public Location GetLocation(String country,string city)
        {        
            foreach (Location location in _locations)
            {
                if (location.Country == country && location.City == city)
                {
                    return location;
                }
            }
            return null;
        }
        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

    }
}
