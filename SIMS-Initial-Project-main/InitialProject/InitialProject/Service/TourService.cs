using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class TourService : ISubject
    {
        private readonly ITourRepository _tourRepository;
        private readonly LocationService _locationService;
        private readonly TourReservationRepository _tourReservationRepository;

        public TourService()
        {
            _tourRepository = Injector.CreateInstance<ITourRepository>();
            _locationService = new LocationService();
            _tourReservationRepository = new TourReservationRepository();
        }

        public Tour GetTourByRequestId(int id)
        {
            Tour requestedTour = new Tour();

            foreach (Tour tour in GetAll())
            {
                if (tour.RequestId == id)
                {
                    requestedTour = tour;
                } 
            }

            return requestedTour;
        }

        public List<Tour> GetUserTours(User user)
        {
            List<Tour> tours = new List<Tour>();
            List<TourReservation> tourReservations = _tourReservationRepository.GetAll();

            foreach (TourReservation tourReservation in tourReservations)
            {
                if (tourReservation.UserId == user.Id && !tourReservation.IsRated)
                {
                    tours.Add(GetById(tourReservation.TourId));
                }
            }
            return tours;
        }
        public List<Tour> GetAllUnabortedGuideTours(User user)
        {
            List<Tour> tours = new List<Tour>();
  
            foreach (Tour tour in GetAll())
            {
                if (tour.GuideId == user.Id && !tour.IsAborted)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }
        public List<Tour> GetFinishedTours(List<Tour> tours)
        {
            return tours.FindAll(tour => tour.IsFinished);
        }

        public bool CheckIfTourCanBeAborted(Tour tour) 
        {
            return tour.StartTime.AddDays(-2) >= DateTime.Now;
        }

        public List<Tour> RemoveFromListById(List<Tour> tours, int id)
        {
            List<Tour> toursRemoved = tours;
            toursRemoved.RemoveAll(t => t.Id == id);
            return toursRemoved;
        }

        public List<Tour> GetTodaysTours(User user)
        {
            var currentDateTime = DateTime.Now;
            var currentTours = new List<Tour>();

            foreach (var tour in GetAllUnabortedGuideTours(user))
            {
                if (tour.StartTime.Date == currentDateTime.Date && tour.StartTime >= currentDateTime && !tour.IsFinished)
                {
                    currentTours.Add(tour);
                }
            }

            return currentTours;
        }
        public List<Tour> GetFinishedTours(User user)
        {
            var finishedTours = new List<Tour>();

            foreach (var tour in GetAllUnabortedGuideTours(user))
            {
                if (tour.IsFinished)
                {
                    finishedTours.Add(tour);
                }
            }

            return finishedTours;
        }
        public List<Tour> GetRatedTours(User user)
        {
            var ratedTours = new List<Tour>();

            foreach (var tour in GetAllUnabortedGuideTours(user))
            {
                if (tour.IsRated)
                {
                    ratedTours.Add(tour);
                }
            }

            return ratedTours;
        }
        public List<Tour> GetUpcomingTours(User user)
        {
            var upcomingTours = new List<Tour>();
            var currentDate = DateTime.Now.Date;

            foreach (var tour in GetAllUnabortedGuideTours(user))
            {
                var tourStartDate = tour.StartTime.Date;
                if (tourStartDate > currentDate )
                {
                    upcomingTours.Add(tour);
                }
            }
            return upcomingTours;
        }
        public List<Tour> GetReservableTours()
        {
            var upcomingTours = new List<Tour>();
            var currentDate = DateTime.Now.Date;

            foreach (var tour in _tourRepository.GetAll())
            {
                var tourStartDate = tour.StartTime.Date;
                if (tourStartDate >= currentDate && !tour.IsAborted && !tour.IsFinished)
                {
                    upcomingTours.Add(tour);
                }
            }
            return upcomingTours;
        }
        public List<Tour> GetByCityName(string city)
        {
            var toursByCityName = new List<Tour>();

            foreach (var tour in GetReservableTours())
            {
                if (_locationService.GetById(tour.LocationId).City == city)
                {
                    toursByCityName.Add(tour);
                }
            }

            return toursByCityName;
        }
        public List<Tour> GetByCountryName(string country)
        {
            var tours = new List<Tour>();

            foreach (var tour in GetReservableTours())
            {
                if (_locationService.GetById(tour.LocationId).Country == country)
                {
                    tours.Add(tour);
                }
            }

            return tours;
        }
        public List<Tour> GetByLanguage(string language)
        {
            var tours = new List<Tour>();

            foreach (var tour in GetReservableTours())
            {
                if (tour.Language == language)
                {
                    tours.Add(tour);
                }
            }

            return tours;
        }
        public List<Tour> GetByDuration(double duration)
        {
            var tours = new List<Tour>();

            foreach (var tour in GetReservableTours())
            {
                if (tour.Duration == duration)
                {
                    tours.Add(tour);
                }
            }

            return tours;
        }
        public List<Tour> GetByGuests(int guestCount)
        {
            var tours = new List<Tour>();

            foreach (var tour in GetReservableTours())
            {
                if (guestCount <= tour.MaxGuests - tour.CurrentGuestCount)
                {
                    tours.Add(tour);
                }
            }

            return tours;
        }
        public List<Tour> GetToursByYear(User user, int year) 
        {
            List<Tour> toursThisYear = new List<Tour>();

            DateTime LeftBoundary = new DateTime(year,1,1);
            DateTime RightBoundary = new DateTime(year, 12, 31);

            foreach (Tour tour in GetFinishedTours(user)) 
            {
                if(tour.StartTime >= LeftBoundary && tour.StartTime <= RightBoundary) 
                {
                    toursThisYear.Add(tour);
                }
            }
            return toursThisYear;
        }
        public List<Tour> GetToursByTimeInterval(User user, DateTime leftBoundary, DateTime rightBoundary)
        {
            List<Tour> tours = new List<Tour>();

            foreach (Tour tour in GetUpcomingTours(user))
            {
                if (tour.StartTime >= leftBoundary.Date && tour.StartTime <= rightBoundary.Date)
                {
                    tours.Add(tour);
                }
            }
            return tours;
        }
        public Tour GetMostVisitedTour(List<Tour> toursThisYear) 
        {
            if (toursThisYear.Count == 0) 
            {
                return null;
            }

            Tour mostVisitedTour = toursThisYear[0];

            foreach (Tour tour in toursThisYear) 
            {
                if(tour.CurrentGuestCount > mostVisitedTour.CurrentGuestCount) 
                {
                    mostVisitedTour = tour;
                }
            }
            return mostVisitedTour;
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

        public List<Tour> GetAll()
        {
            return _tourRepository.GetAll();
        }
    }
}
