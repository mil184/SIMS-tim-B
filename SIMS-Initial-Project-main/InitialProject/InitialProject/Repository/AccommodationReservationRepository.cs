using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class AccommodationReservationRepository : ISubject
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;

        private readonly List<IObserver> _observers;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();

            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            NotifyObservers();

            return accommodationReservation;
        }

        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservations;
        }

        public AccommodationReservation Create(int accommodationId, DateTime startDate, DateTime endDate)
        {
            var availableDates = GetAvailableDates(accommodationId, startDate, endDate);

            if (availableDates.Count == (endDate - startDate).Days + 1)
            {
                var reservation = new AccommodationReservation()
                {
                    AccommodationId = accommodationId,
                    StartDate = startDate,
                    EndDate = endDate,
                };

                Save(reservation);

                return reservation;
            }

            return null;
        }
        public List<DateTime> GetAvailableDates(int accommodationId, DateTime startDate, DateTime endDate)
        {
            List<DateTime> reservedDates = new List<DateTime>();
            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                if (reservation.AccommodationId == accommodationId && reservation.StartDate >= startDate && reservation.StartDate <= endDate)
                {
                    for (DateTime date = reservation.StartDate; date <= reservation.EndDate; date = date.AddDays(1))
                    {
                        reservedDates.Add(date);
                    }
                }
            }
            List<DateTime> availableDates = new List<DateTime>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!reservedDates.Contains(date))
                {
                    availableDates.Add(date);
                }
            }
            return availableDates;
        }
        /*  public void MarkUnavailable(int accommodationId, DateTime startDate, DateTime endDate)
          {
            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
              if (reservation.AccommodationId == accommodationId && reservation.StartDate >= startDate && reservation.EndDate <= endDate)
              {
                reservation.IsAvailable = false;
              }
            }
            _serializer.ToCSV(_filePath, _accommodationReservations);
            NotifyObservers();
          }*/

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
