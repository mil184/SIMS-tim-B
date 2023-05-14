using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class LocationService: ISubject
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService()
        {
           _locationRepository = Injector.CreateInstance<ILocationRepository>();    
        }
        public List<string> GetCountries()
        {
            List<string> countries = new List<string>();
            foreach (Location location in _locationRepository.GetAll())
            {
                if (!countries.Contains(location.Country))
                    countries.Add(location.Country);
            }
            return countries;
        }
        public List<string> GetCities()
        {
            List<string> cities = new List<string>();
            foreach (Location location in _locationRepository.GetAll())
            {
                if (!cities.Contains(location.City))
                    cities.Add(location.City);
            }
            return cities;
        }

        public List<string> GetCitiesByCountry(String country)
        {
            List<string> cities = new List<string>();
            foreach (Location location in _locationRepository.GetAll())
            {
                if (location.Country == country)
                {
                    cities.Add(location.City);
                }
            }
            return cities;
        }
        public string GetCountryByCity(String city)
        {
            string country = string.Empty;

            foreach (Location location in _locationRepository.GetAll())
            {
                if (location.City == city)
                {
                    country = location.Country;
                }
            }
            return country;
        }
        public Location GetLocation(String country, string city)
        {
            foreach (Location location in _locationRepository.GetAll())
            {
                if (location.Country == country && location.City == city)
                {
                    return location;
                }
            }
            return null;
        }
        public Location GetById(int id)
        {
            return _locationRepository.GetById(id);
        }

        public void NotifyObservers()
        {
            _locationRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _locationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _locationRepository.Unsubscribe(observer);
        }
    }
}
