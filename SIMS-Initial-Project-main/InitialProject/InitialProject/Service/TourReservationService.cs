using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class TourReservationService
    {
        private readonly TourReservationRepository _tourReservationRepository;

        public TourReservationService()
        {
            _tourReservationRepository = new TourReservationRepository();
        }

        public List<int> GetUserIdsByTour(Tour tour)
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
