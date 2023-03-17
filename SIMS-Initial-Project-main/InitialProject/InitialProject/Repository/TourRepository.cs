using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using InitialProject.View.Guide;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;

namespace InitialProject.Repository
{
    public class TourRepository : ISubject
    {
        private const string _filePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<Tour> _serializer;
        private readonly LocationRepository _locationRepository;
        private readonly List<IObserver> _observers;

        private List<Tour> _tours;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(_filePath);
            _locationRepository = new LocationRepository();
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(_filePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(c => c.Id) + 1;
        }

        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _serializer.FromCSV(_filePath);
            _tours.Add(tour);
            _serializer.ToCSV(_filePath, _tours);
            NotifyObservers();

            return tour;
        }
        public Tour Update(Tour tour)
        {
            _tours = _serializer.FromCSV(_filePath);
            Tour current = _tours.Find(c => c.Id == tour.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tour);       // keep ascending order of ids in file 
            _serializer.ToCSV(_filePath, _tours);
            return tour;
        }
        public Tour GetById(int id)
        {
            _tours = _serializer.FromCSV(_filePath);
            return _tours.Find(c => c.Id == id);
        }

        public List<Tour> GetAll()
        {
            var tours = new List<Tour>();

            foreach (var tour in _tours)
            {
                tours.Add(tour);
            }

            return tours;
        }

        public List<Tour> GetTodaysTours()
        {
            var currentDateTime = DateTime.Now;
            var currentTours = new List<Tour>();

            foreach (var tour in _tours)
            {
                if (tour.StartTime.Date == currentDateTime.Date && tour.StartTime >= currentDateTime)
                {
                    currentTours.Add(tour);
                }
            }

            return currentTours;
        }

        public List<Tour> GetUpcomingTours()
        {
            var upcomingTours = new List<Tour>();
            var currentDate = DateTime.Now.Date;

            foreach (var tour in _tours)
            {
                var tourStartDate = tour.StartTime.Date;
                if (tourStartDate > currentDate)
                {
                    upcomingTours.Add(tour);
                }
            }
            return upcomingTours;
        }

        public List<Tour> GetByCity(Location location)
        {
            var toursByCity = new List<Tour>();

            foreach (var tour in _tours)
            {
                if (_locationRepository.GetById(tour.LocationId).City == location.City)
                {
                    toursByCity.Add(tour);
                }
            }

            return toursByCity;
        }

        public List<Tour> GetByCityName(string city)
        {
            var toursByCityName = new List<Tour>();

            foreach (var tour in _tours)
            {
                if (_locationRepository.GetById(tour.LocationId).City == city)
                {
                    toursByCityName.Add(tour);
                }
            }

            return toursByCityName;
        }

        public List<Tour> GetByCountry(Location location)
        {
            var toursByCountry = new List<Tour>();

            foreach (var tour in _tours)
            {
                if (_locationRepository.GetById(tour.LocationId).Country == location.Country)
                {
                    toursByCountry.Add(tour);
                }
            }

            return toursByCountry;
        }

        public bool HasAvailableSpace(Tour tour, int guestsToAdd)
        {
            return (tour.CurrentGuestCount + guestsToAdd < tour.MaxGuests);
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
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
