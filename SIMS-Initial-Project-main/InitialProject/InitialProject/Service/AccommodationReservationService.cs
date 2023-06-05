using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
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

        private readonly IAccommodationReservationRepository _accommodationReservationRepository;
        private readonly AccommodationRenovationService _accommodationRenovationService;
        private readonly AccommodationRepository _accommodationRepository;

        public AccommodationReservationService()
        {
            _accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
            _accommodationRenovationService = new AccommodationRenovationService();
            _accommodationRepository = new AccommodationRepository();
        }

        public (int, int) GetBusiestLocation(int ownerId)
        {
            List<Accommodation> accommodations = new List<Accommodation>();
            foreach (AccommodationReservation reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.OwnerId == ownerId)
                {
                    accommodations.Add(_accommodationRepository.GetById(reservation.AccommodationId));
                }
            }

            Dictionary<int, int> locationCounts = new Dictionary<int, int>();
            foreach (Accommodation accommodation in accommodations)
            {
                int locationId = accommodation.LocationId;
                if (locationCounts.ContainsKey(locationId))
                {
                    locationCounts[locationId]++;
                }
                else
                {
                    locationCounts[locationId] = 1;
                }
            }

            int maxLocation = 0;
            int locationWithMostVisits = 0;

            int minLocation = int.MaxValue;
            int locationWithLeastVisits = 0;

            foreach (var kvp in locationCounts)
            {
                if (kvp.Value > maxLocation)
                {
                    maxLocation = kvp.Value;
                    locationWithMostVisits = kvp.Key;
                }

                if (kvp.Value < minLocation)
                {
                    minLocation = kvp.Value;
                    locationWithLeastVisits = kvp.Key;
                }
            }

            return (locationWithMostVisits, locationWithLeastVisits);
        }

        public int GetBusiestYear(int accommodationId)
        {
            var yearCounts = new Dictionary<int, int>();

            // Iterate over each reservation
            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.AccommodationId == accommodationId)
                {
                    // Extract the year from the reservation date
                    int year = reservation.StartDate.Year;

                    // Add the number of reserved days to the corresponding year's count
                    if (yearCounts.ContainsKey(year))
                    {
                        yearCounts[year] += reservation.NumberDays;
                    }
                    else
                    {
                        yearCounts[year] = reservation.NumberDays;
                    }
                }
            }

            // Find the year with the maximum total number of reserved days
            int maxYear = 0;
            int maxCount = 0;

            foreach (var entry in yearCounts)
            {
                if (entry.Value > maxCount)
                {
                    maxYear = entry.Key;
                    maxCount = entry.Value;
                }
            }

            return maxYear;
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
            if(renovation != null)
            {
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

        public List<AccommodationReservation> GetReservationsByMonth(int accommodationId, int year, int month)
        {
            List<AccommodationReservation> reservationsThisYear = new List<AccommodationReservation>();

            int day;

            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                day = 31;
            }
            else if (month == 2)
            {
                day = 28;
            }
            else
            {
                day = 30;
            }

            DateTime LeftBoundary = new DateTime(year, month, 1);
            DateTime RightBoundary = new DateTime(year, month, day);

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

        public List<AccommodationReservation> GetByAccommodationId(int accommodationId)
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();
            foreach(AccommodationReservation reservation in _accommodationReservationRepository.GetAll())
            {
                if(reservation.AccommodationId == accommodationId)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }

        public List<AccommodationReservation> GetReservationsByGuestId(int guestId)
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.GuestId == guestId)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }

        public bool HasGuestVisitedLocation(int guestId)
        {
            List<AccommodationReservation> reservations = GetReservationsByGuestId(guestId);

            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.HasGuestPresence)
                {
                    return true;
                }
            }

            return false;
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
