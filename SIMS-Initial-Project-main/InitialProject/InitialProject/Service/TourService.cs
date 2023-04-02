using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourService : ISubject
    {
        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;

        public TourService()
        {
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
        }

        public List<Tour> RemoveFromListById(List<Tour> tours, int id)
        {
            List<Tour> toursRemoved = tours;
            toursRemoved.RemoveAll(t => t.Id == id);
            return toursRemoved;
        }
        public List<Tour> GetTodaysTours()
        {
            var currentDateTime = DateTime.Now;
            var currentTours = new List<Tour>();

            foreach (var tour in _tourRepository.GetAll())
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

            foreach (var tour in _tourRepository.GetAll())
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

            foreach (var tour in _tourRepository.GetAll())
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

            foreach (var tour in _tourRepository.GetAll())
            {
                if (_locationRepository.GetById(tour.LocationId).City == city)
                {
                    toursByCityName.Add(tour);
                }
            }

            return toursByCityName;
        }
        public bool HasAvailableSpace(Tour tour, int guestsToAdd)
        {
            return (tour.CurrentGuestCount + guestsToAdd < tour.MaxGuests);
        }
        public List<Tour> GetByCountry(Location location)
        {
            var toursByCountry = new List<Tour>();

            foreach (var tour in _tourRepository.GetAll())
            {
                if (_locationRepository.GetById(tour.LocationId).Country == location.Country)
                {
                    toursByCountry.Add(tour);
                }
            }

            return toursByCountry;
        }
        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }

        public void Subscribe(IObserver observer)
        {
            _tourRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _tourRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _tourRepository.NotifyObservers();
        }

        public void Update(Tour tour)
        {
            _tourRepository.Update(tour);
        }

        public Tour Save(Tour tour)
        {
            return _tourRepository.Save(tour);
        }

        internal List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }
    }
}
