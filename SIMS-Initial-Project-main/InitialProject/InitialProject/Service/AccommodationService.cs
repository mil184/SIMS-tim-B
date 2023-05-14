﻿using InitialProject.Model;
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

        public AccommodationService()
        {
            _accommodationRepository = new AccommodationRepository();
            _locationService = new LocationService();
            _accommodationRenovationService = new AccommodationRenovationService();

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
