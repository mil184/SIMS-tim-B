using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Service
{
    public class GuestReviewService : ISubject
    {
        private readonly IGuestReviewRepository _guestReviewRepository;
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _reservationService;
        private readonly UserService _userService;

        public GuestReviewService()
        {
            _guestReviewRepository = Injector.CreateInstance<IGuestReviewRepository>();
            _accommodationService = new AccommodationService();
            _reservationService = new AccommodationReservationService();
            _userService = new UserService();
        }

        public List<GuestReviewDTO> GetUnreviewedGuests(int ownerId)
        {
            var guests = new List<GuestReviewDTO>();

            foreach (AccommodationReservation reservation in _reservationService.GetAll())
            {
                if (LessThanFiveDaysPassed(reservation) && !GuestReviewed(reservation))
                {
                    if (_accommodationService.GetByUser(ownerId).Any(item => item.Id == reservation.AccommodationId))
                    {
                        guests.Add(new GuestReviewDTO(reservation.Id, _userService.GetById(reservation.GuestId).Username, _accommodationService.GetById(reservation.AccommodationId).Name));
                    }
                }
            }

            return guests;
        }

        private bool GuestReviewed(AccommodationReservation reservation)
        {
            return _guestReviewRepository.GetAll().Any(item => item.ReservationId == reservation.Id);
        }

        private bool LessThanFiveDaysPassed(AccommodationReservation reservation)
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(reservation.EndDate);
            return timeSpan.Days >= 0 && timeSpan.Days < 5;
        }

        public GuestReview Save(GuestReview guestReview)
        {
            return _guestReviewRepository.Save(guestReview);
        }

        public List<GuestReview> GetAll()
        {
            return _guestReviewRepository.GetAll();
        }

        public void Subscribe(IObserver observer)
        {
            _guestReviewRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _guestReviewRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _guestReviewRepository.NotifyObservers();
        }
    }
}
