using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class ReservationCancellationService
    {
        private readonly IReservationCancellationRepository _reservationCancellationRepository;
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _reservationService;
        private readonly UserService _userService;

        public ReservationCancellationService()
        {
            _reservationCancellationRepository = Injector.CreateInstance<IReservationCancellationRepository>();
            _accommodationService = new AccommodationService();
            _reservationService = new AccommodationReservationService();
            _userService = new UserService();
        }

        public List<ReservationCancellation> GetCancellationsByYear(int accommodationId, int year)
        {
            List<ReservationCancellation> cancellationsThisYear = new List<ReservationCancellation>();

            DateTime LeftBoundary = new DateTime(year, 1, 1);
            DateTime RightBoundary = new DateTime(year, 12, 31);

            foreach (ReservationCancellation cancellation in _reservationCancellationRepository.GetAll())
            {
                if (cancellation.AccommodationId == accommodationId && cancellation.CancellationDate >= LeftBoundary && cancellation.CancellationDate <= RightBoundary)
                {
                    cancellationsThisYear.Add(cancellation);
                }
            }
            return cancellationsThisYear;
        }

        public List<ReservationCancellation> GetCancellationsByMonth(int accommodationId, int year, int month)
        {
            List<ReservationCancellation> cancellationsThisYear = new List<ReservationCancellation>();

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

            foreach (ReservationCancellation cancellation in _reservationCancellationRepository.GetAll())
            {
                if (cancellation.AccommodationId == accommodationId && cancellation.CancellationDate >= LeftBoundary && cancellation.CancellationDate <= RightBoundary)
                {
                    cancellationsThisYear.Add(cancellation);
                }
            }
            return cancellationsThisYear;
        }

        public ReservationCancellation Save(ReservationCancellation reservationCancellation)
        {
            return _reservationCancellationRepository.Save(reservationCancellation);
        }

        public List<ReservationCancellation> GetAll()
        {
            return _reservationCancellationRepository.GetAll();
        }

        public void Subscribe(IObserver observer)
        {
            _reservationCancellationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _reservationCancellationRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _reservationCancellationRepository.NotifyObservers();
        }
    }
}
