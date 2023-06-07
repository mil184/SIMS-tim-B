using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System.Collections.Generic;
using System.Windows;

namespace InitialProject.Service
{
    public class TourReservationService
    {
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly VoucherService _voucherService;

        public TourReservationService()
        {
            _tourReservationRepository = new TourReservationRepository();
            _voucherService = new VoucherService();
        }

        public Voucher AcquireVoucher(TourReservation tourReservation)
        {
            int year = tourReservation.ReservationTime.Year;
            List<TourReservation> reservationList = new List<TourReservation>();

            foreach(var reservation in GetAllWithUnacquiredVoucher())
            {
                if(reservation.ReservationTime.Year == year)
                {
                    reservationList.Add(reservation);
                }
            }

            if (reservationList.Count == 5)
            {
                Voucher voucher = new Voucher("5 tours", System.DateTime.Now, System.DateTime.Now.AddMonths(6), tourReservation.UserId);
                _voucherService.Save(voucher);
                MessageBox.Show("You've recieved a voucher!");

                foreach(var reservation in reservationList)
                {
                    reservation.GotVoucher = true;
                    Update(reservation);
                }

                return voucher;
            }

            return null;
        }

        public List<TourReservation> GetAllWithUnacquiredVoucher()
        {
            List<TourReservation> reservationList = new List<TourReservation>();

            foreach (var reservation in GetAll())
            {
                if (!reservation.GotVoucher)
                {
                    reservationList.Add(reservation);
                }
            }

            return reservationList;
        }
           

        public List<int> GetUncheckedUserIdsByTour(Tour tour)
        {
            List<int> userIds = new List<int>();
            List <TourReservation> _tourReservations = _tourReservationRepository.GetAll();

            foreach (TourReservation reservation in _tourReservations)
            {
                if (reservation.TourId == tour.Id && !reservation.GuestChecked)
                {
                    userIds.Add(reservation.UserId);
                }
            }
            return userIds;
        }
        public List<int> GetAllUserIdsByTour(Tour tour)
        {
            List<int> userIds = new List<int>();
            List<TourReservation> _tourReservations = _tourReservationRepository.GetAll();

            foreach (TourReservation reservation in _tourReservations)
            {
                if (reservation.TourId == tour.Id)
                {
                    userIds.Add(reservation.UserId);
                }
            }
            return userIds;
        }
        public List<int> GetCheckedTourIds(User user)
        {
            List<int> tourIds = new List<int>();
            List<TourReservation> _tourReservations = _tourReservationRepository.GetAll();


            foreach (TourReservation reservation in _tourReservations)
            {
                if (reservation.UserId == user.Id && reservation.GuestChecked)
                {
                    tourIds.Add(reservation.TourId);
                }
            }

            return tourIds;
        }

        public TourReservation GetReservationByGuestIdAndTourId(int guestId, int tourId)
        {
            List<TourReservation> _tourReservations = _tourReservationRepository.GetAll();

            foreach (TourReservation tourReservation in _tourReservations)
            {
                if (guestId == tourReservation.UserId && tourId == tourReservation.TourId)
                {
                    return tourReservation;
                }
            }
            return null;
        }

        public List<TourReservation> GetReservationsByTourId(int tourId)
        {
            List<TourReservation> tourReservations = new List<TourReservation>();

            foreach (TourReservation tourReservation in GetAll())
            {
                if (tourId == tourReservation.TourId)
                { 
                    tourReservations.Add(tourReservation);
                }
            }
            return tourReservations;
        }
        public int GetUnder18Count(Tour tour)
        {
            List<TourReservation> _tourReservations = _tourReservationRepository.GetAll();
            int counter = 0;

            foreach (TourReservation reservation in _tourReservations)
            {
                if (tour.Id == reservation.TourId && reservation.AverageAge < 18)
                {
                    counter++;
                }
            }
            return counter;
        }

        public int GetBetween18And50Count(Tour tour)
        {
            List<TourReservation> _tourReservations = _tourReservationRepository.GetAll();
            int counter = 0;
            foreach (TourReservation reservation in _tourReservations)
            {
                if (tour.Id == reservation.TourId && reservation.AverageAge >= 18 && reservation.AverageAge <= 50)
                {
                    counter++;
                }
            }
            return counter;
        }
        public int GetAbove50Count(Tour tour)
        {
            List<TourReservation> _tourReservations = _tourReservationRepository.GetAll();
            int counter = 0;
            foreach (TourReservation reservation in _tourReservations)
            {
                if (tour.Id == reservation.TourId && reservation.AverageAge > 50)
                {
                    counter++;
                }
            }
            return counter;
        }
        public int GetUsedVoucherCount(Tour tour)
        {
            List<TourReservation> _tourReservations = _tourReservationRepository.GetAll();
            int counter = 0;
            foreach (TourReservation reservation in _tourReservations)
            {
                if (tour.Id == reservation.TourId && reservation.UsedVoucherId != -1)
                {
                    counter++;
                }
            }
            return counter;
        }
        public int GetUnusedVoucherCount(Tour tour)
        {
            List<TourReservation> _tourReservations = _tourReservationRepository.GetAll();
            int counter = 0;
            foreach (TourReservation reservation in _tourReservations)
            {
                if (tour.Id == reservation.TourId && reservation.UsedVoucherId == -1)
                {
                    counter++;
                }
            }
            return counter;
        }

        public TourReservation Update(TourReservation tourReservation)
        {
            return _tourReservationRepository.Update(tourReservation);
        }

        public TourReservation Save(TourReservation tourReservation)
        {
            return _tourReservationRepository.Save(tourReservation);
        }

        public List<TourReservation> GetAll()
        {
            return _tourReservationRepository.GetAll();
        }

        public TourReservation GetByTourId(int id)
        {
            return _tourReservationRepository.GetByTourId(id);
        }

        public void Subscribe(IObserver observer)
        {
            _tourReservationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _tourReservationRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _tourReservationRepository.NotifyObservers();
        }
    }
}
