using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Enums;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class AccommodationService
    {
        private readonly AccommodationRepository _accommodationRepository;
        private readonly LocationService _locationService;
        private readonly AccommodationRenovationService _accommodationRenovationService;
        private readonly AccommodationReservationService _accommodationReservationService;

        public AccommodationService()
        {
            _accommodationRepository = new AccommodationRepository();
            _locationService = new LocationService();
            _accommodationRenovationService = new AccommodationRenovationService();
            _accommodationReservationService = new AccommodationReservationService();

            SetRenovatedStatus();
        }

        private void SetRenovatedStatus()
        {
            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                if (_accommodationRenovationService.GetAll().Any(r => r.AccommodationId == accommodation.Id && r.EndDate.AddYears(1) > DateTime.Now))
                {
                    accommodation.IsRenovated = true;
                    _accommodationRepository.Update(accommodation);
                }
            }
        }

        public List<int> GetAllLocations(int ownerId)
        {
            List<int> retval = new List<int>();

            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.OwnerId == ownerId)
                {
                    retval.Add(accommodation.LocationId);
                }
            }

            return retval;
        }

        public Accommodation GetAccommodationByLocation(int locationId)
        {
            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.LocationId == locationId)
                {
                    return accommodation;
                }
            }
            return null;
        }

        public List<Accommodation> GetByUser(int ownerId)
        {
            var accommodations = new List<Accommodation>();

            foreach(var accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.OwnerId == ownerId)
                {
                    accommodations.Add(accommodation);
                }
            }

            return accommodations;
        }

        public List<Accommodation> GetByName(string name)
        {
            var accommodations = new List<Accommodation>();

            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.Name == name)
                {
                    accommodations.Add(accommodation);
                }
            }

            return accommodations;
        }

        public List<Accommodation> GetAccommodationsByLocation(int ownerId, int locationId)
        {
            var accommodations = new List<Accommodation>();

            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.OwnerId == ownerId && accommodation.LocationId == locationId)
                {
                    accommodations.Add(accommodation);
                }
            }

            return accommodations;
        }

        public List<Accommodation> GetByCityName(string city)
        {
            var accommodationsByCityName = new List<Accommodation>();

            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                if (_locationService.GetById(accommodation.LocationId).City == city)
                {
                    accommodationsByCityName.Add(accommodation);
                }
            }

            return accommodationsByCityName;
        }
        public List<Accommodation> GetByCountryName(string country)
        {
            var accommodations = new List<Accommodation>();

            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                if (_locationService.GetById(accommodation.LocationId).Country == country)
                {
                    accommodations.Add(accommodation);
                }
            }

            return accommodations;
        }
        public List<Accommodation> GetByMaxGuests(int guest)
        {
            var accommodations = new List<Accommodation>();

            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.MaxGuests == guest)
                {
                    accommodations.Add(accommodation);
                }
            }

            return accommodations;
        }

        public List<Accommodation> GetByMinDays(int day)
        {
            var accommodations = new List<Accommodation>();

            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.MinReservationDays == day)
                {
                    accommodations.Add(accommodation);
                }
            }

            return accommodations;
        }

        public List<Accommodation> GetByType(string type)
        {
            var accommodations = new List<Accommodation>();

            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.Type.ToString() == type)
                {
                    accommodations.Add(accommodation);
                }
            }

            return accommodations;
        }

        public List<Accommodation> GetAvailableAccommodations(DateTime startDate, DateTime endDate, int numberOfGuests, int numberOfDays)
        {
            List<Accommodation> availableAccommodations = new List<Accommodation>();
            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                bool isAvailable = IsAccommodationAvailable(accommodation, startDate, endDate);
                bool hasEnoughGuestCapacity = accommodation.MaxGuests >= numberOfGuests;
                bool meetsMinReservationDays = accommodation.MinReservationDays >= numberOfDays;

                if (isAvailable && hasEnoughGuestCapacity && meetsMinReservationDays)
                {
                    availableAccommodations.Add(accommodation);
                }
            }
            return availableAccommodations;
        }

        public bool IsAccommodationAvailable(Accommodation accommodation, DateTime startDate, DateTime endDate)
        {
            List<AccommodationReservation> reservations = _accommodationReservationService.GetByAccommodationId(accommodation.Id);
            foreach(AccommodationReservation reservation in reservations)
            {
                if(startDate <= reservation.EndDate && endDate >= reservation.StartDate)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Accommodation> GetAvailable(int numberOfGuests, int numberOfDays)
        {
            List<Accommodation> availableAccommodations = new List<Accommodation>();
            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                bool hasEnoughGuestCapacity = accommodation.MaxGuests >= numberOfGuests;
                bool meetsMinReservationDays = accommodation.MinReservationDays >= numberOfDays;

                if (hasEnoughGuestCapacity && meetsMinReservationDays)
                {
                    availableAccommodations.Add(accommodation);
                }
            }
            return availableAccommodations;
        }

     /*   public bool HasOwnerAccommodationOnLocation(int ownerId)
        {
            List<Accommodation> accommodations = GetByUser(ownerId);

            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.HasAccommodation)
                {
                    return true;
                }
            }
            return false;
        }*/

        public Accommodation GetById(int id)
        {
            return _accommodationRepository.GetById(id);
        }

        public void Subscribe(IObserver observer)
        {
            _accommodationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _accommodationRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _accommodationRepository.NotifyObservers();
        }

        public Accommodation Save(Accommodation accommodation)
        {
            return _accommodationRepository.Save(accommodation);
        }

        internal List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }
    }
}
