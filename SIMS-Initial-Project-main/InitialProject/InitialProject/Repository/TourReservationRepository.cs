using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class TourReservationRepository : ISubject
    {
        private const string _filePath = "../../../Resources/Data/tourReservations.csv";

        private readonly Serializer<TourReservation> _serializer;
        private readonly List<IObserver> _observers;

        private List<TourReservation> _tourReservations;

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();
            _tourReservations = _serializer.FromCSV(_filePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _tourReservations = _serializer.FromCSV(_filePath);
            if (_tourReservations.Count < 1)
            {
                return 1;
            }
            return _tourReservations.Max(c => c.Id) + 1;
        }

        public TourReservation Save(TourReservation tourReservation)
        {
            tourReservation.Id = NextId();
            _tourReservations = _serializer.FromCSV(_filePath);
            _tourReservations.Add(tourReservation);
            _serializer.ToCSV(_filePath, _tourReservations);
            NotifyObservers();

            return tourReservation;
        }
        public TourReservation Update(TourReservation reservation)
        {
            _tourReservations = _serializer.FromCSV(_filePath);
            TourReservation current = _tourReservations.Find(c => c.Id == reservation.Id);
            int index = _tourReservations.IndexOf(current);
            _tourReservations.Remove(current);
            _tourReservations.Insert(index, reservation);       // keep ascending order of ids in file 
            _serializer.ToCSV(_filePath, _tourReservations);
            return reservation;
        }
        public List<int> GetUserIdsByTour(Tour tour) 
        {
            List<int> userIds = new List<int>();

            foreach(TourReservation reservation in _tourReservations) 
            {
                if(reservation.TourId == tour.Id) 
                {
                    userIds.Add(reservation.UserId);
                }   
            }
            return userIds;
        }
        public TourReservation GetReservationByGuestIdAndTourId(int guestId, int tourId)
        {
            foreach(TourReservation tourReservation in _tourReservations) 
            {
                if(guestId == tourReservation.UserId && tourId == tourReservation.TourId) 
                {
                    return tourReservation;
                }
            }
            return null;
        }

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
