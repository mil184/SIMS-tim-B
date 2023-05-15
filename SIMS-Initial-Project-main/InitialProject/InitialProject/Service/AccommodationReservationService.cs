using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Service
{
    public class AccommodationReservationService
    {

        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        private readonly AccommodationRenovationService _accommodationRenovationService;

        public AccommodationReservationService()
        {
            _accommodationReservationRepository = new AccommodationReservationRepository();
            _accommodationRenovationService = new AccommodationRenovationService();
        }

        public List<DateTime> GetAvailableDates(int accommodationId, int duration, DateTime startDate, DateTime endDate)
        {
            List<DateTime> reservedDates = new List<DateTime>();
            foreach (AccommodationReservation reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.AccommodationId == accommodationId && reservation.StartDate >= startDate && reservation.StartDate <= endDate)
                {
                    for (DateTime date = reservation.StartDate; date <= reservation.EndDate; date = date.AddDays(1))
                    {
                        reservedDates.Add(date);
                    }
                }
            }
            #region renovation
            AccommodationRenovation renovation = _accommodationRenovationService.GetByAccommodationId(accommodationId);     // renovation of requested accommodation
            if (renovation.StartDate >= startDate && renovation.StartDate <= endDate)                                       // adds to reserved dates all renovation dates
            { 
                for (DateTime date = renovation.StartDate; date <= renovation.EndDate; date = date.AddDays(1))
                {
                    if (!reservedDates.Contains(date))
                    {
                        reservedDates.Add(date);
                    }
                }
            }
            if (renovation.EndDate >= startDate && renovation.EndDate <= endDate)
            {
                for (DateTime date = renovation.StartDate; date <= renovation.EndDate; date = date.AddDays(1))
                {
                    if (!reservedDates.Contains(date))
                    {
                        reservedDates.Add(date);
                    }
                }
            }
            #endregion
            List<DateTime> availableDates = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!reservedDates.Contains(date))
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

        public List<AccommodationReservation> GetReservationsByYear(int accommodationId, int year)
        {
            List<AccommodationReservation> reservationsThisYear = new List<AccommodationReservation>();

            DateTime LeftBoundary = new DateTime(year, 1, 1);
            DateTime RightBoundary = new DateTime(year, 12, 31);

            foreach (AccommodationReservation reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.AccommodationId == accommodationId && reservation.StartDate >= LeftBoundary && reservation.StartDate <= RightBoundary)
                {
                    reservationsThisYear.Add(reservation);
                }
            }
            return reservationsThisYear;
        }

        public List<AccommodationReservation> GetUnratedAccommodations()
        {
            List<AccommodationReservation> unratedAccommodations = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _accommodationReservationRepository.GetAll())
            {
                if (!reservation.IsRated)
                {
                    unratedAccommodations.Add(reservation);
                }
            }
            return unratedAccommodations;
        }

        public AccommodationReservation GetById(int id)
        {
            return _accommodationReservationRepository.GetById(id);
        }

        public void Subscribe(IObserver observer)
        {
            _accommodationReservationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _accommodationReservationRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _accommodationReservationRepository.NotifyObservers();
        }

        public AccommodationReservation Save(AccommodationReservation reservation)
        {
            return _accommodationReservationRepository.Save(reservation);
        }
        public void Update(AccommodationReservation reservation)
        {
            _accommodationReservationRepository.Update(reservation);
        }

        public void Remove(AccommodationReservation reservation)
        {
            _accommodationReservationRepository.Remove(reservation);
        }

        internal List<AccommodationReservation> GetAll()
        {
            return _accommodationReservationRepository.GetAll();
        }
    }
}
