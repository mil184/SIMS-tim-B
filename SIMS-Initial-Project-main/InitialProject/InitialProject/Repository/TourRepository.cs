using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using InitialProject.View.Guide;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace InitialProject.Repository
{
    public class TourRepository: ISubject
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<Tour> _serializer;

        private readonly List<IObserver> _observers;
        private readonly LocationRepository _locationRepository; 

        private List<Tour> _tours;
        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
            _locationRepository = new LocationRepository();
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(c => c.Id) + 1;
        }

        public Tour Save(Tour tour)
        {
            tour.Id = NextId();

            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tour);
            _serializer.ToCSV(FilePath, _tours);
            NotifyObservers();

            return tour;
        }
        public List<Tour> GetTodaysTours()
        {
            DateTime currentDate = DateTime.Now.Date; // get current date only
            List<Tour> currentTours = new List<Tour>();

            foreach (Tour tour in _tours)
            {
                DateTime tourStartDate = tour.StartTime.Date; // get tour start date only

                if (currentDate == tourStartDate)
                {
                    currentTours.Add(tour);
                }
            }
            return currentTours;
        }

        public List<Tour> GetUpcomingTours()
        {
            List<Tour> upcomingTours = new List<Tour>();
            DateTime currentDate = DateTime.Now.Date;
            foreach (Tour tour in _tours)
            {
                DateTime tourStartDate = tour.StartTime.Date;
                if (tourStartDate > currentDate)
                {
                    upcomingTours.Add(tour);
                }
            }

            return upcomingTours;
        }
        public List<Tour> GetByCity(Location location)
        {
            List<Tour> toursByCity = new List<Tour>();

            foreach (Tour tour in _tours) 
            {
                if (_locationRepository.GetById(tour.LocationId).City == location.City) 
                {
                    toursByCity.Add(tour);
                }
            }
            return toursByCity;
        }
        public List<Tour> GetByCountry(Location location)
        {
            List<Tour> toursByCountry = new List<Tour>();

            foreach (Tour tour in _tours)
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
