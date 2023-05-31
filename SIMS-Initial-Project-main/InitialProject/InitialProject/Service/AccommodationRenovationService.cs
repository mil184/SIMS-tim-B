using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Service
{
    public class AccommodationRenovationService : ISubject
    {
        private readonly IAccommodationRenovationRepository _accommodationRenovationRepository; 
        private readonly AccommodationReservationRepository _accommodationReservationRepository;

        public AccommodationRenovationService()
        {
            _accommodationRenovationRepository = Injector.CreateInstance<IAccommodationRenovationRepository>();
            _accommodationReservationRepository = new AccommodationReservationRepository();
        }

        public List<AccommodationRenovation> GetUpcomingRenovations(int ownerId)
        {
            List<AccommodationRenovation> upcomingRenovations = new List<AccommodationRenovation>();

            foreach (AccommodationRenovation renovation in _accommodationRenovationRepository.GetAll())
            {
                if (renovation.OwnerId == ownerId && renovation.StartDate >= DateTime.Now)
                {
                    upcomingRenovations.Add(renovation);
                }
            }

            return upcomingRenovations;
        }

        public List<AccommodationRenovation> GetFinishedRenovations(int ownerId)
        {
            List<AccommodationRenovation> finishedRenovations = new List<AccommodationRenovation>();

            foreach (AccommodationRenovation renovation in _accommodationRenovationRepository.GetAll())
            {
                if (renovation.OwnerId == ownerId && renovation.EndDate <= DateTime.Now)
                {
                    finishedRenovations.Add(renovation);
                }
            }

            return finishedRenovations;
        }

        public List<DateTime> GetAvailableDates(int accommodationId, int duration, DateTime startDate, DateTime endDate)
        {
            List<DateTime> availableDates = new List<DateTime>();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                bool isAvailable = true;

                foreach (AccommodationReservation reservation in _accommodationReservationRepository.GetAll())
                {
                    if(reservation.AccommodationId == accommodationId)
                    {
                        if (date >= reservation.StartDate && date <= reservation.EndDate)
                        {
                            isAvailable = false;
                            break;
                        }
                    }
                }

                if (isAvailable)
                {
                    availableDates.Add(date);
                }
            }

            List<DateTime> filteredDates = new List<DateTime>();

            foreach (DateTime date in availableDates)
            {
                bool isAvailable = true;
                
                for (int i = 0; i < duration; i++)
                {
                    if (!availableDates.Contains(date.AddDays(i)))
                    {
                        isAvailable = false;
                        break;
                    }
                }

                if (isAvailable)
                {
                    filteredDates.Add(date);
                }
            }

            return filteredDates;
        }

        public AccommodationRenovation GetByAccommodationId(int accommodationId)
        {
            return _accommodationRenovationRepository.GetAll().FirstOrDefault(x => x.AccommodationId == accommodationId);
        }

        public void Remove(int id)
        {
            _accommodationRenovationRepository.Remove(id);
        }

        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            return _accommodationRenovationRepository.Save(accommodationRenovation);
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _accommodationRenovationRepository.GetAll();
        }

        public AccommodationRenovation GetById(int id)
        {
            return _accommodationRenovationRepository.GetById(id);
        }

        public void Subscribe(IObserver observer)
        {
            _accommodationRenovationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _accommodationRenovationRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _accommodationRenovationRepository.NotifyObservers();
        }
    }
}
