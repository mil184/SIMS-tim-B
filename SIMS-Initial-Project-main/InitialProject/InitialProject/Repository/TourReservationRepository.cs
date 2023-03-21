using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public List<TourReservation> RemoveById(List<TourReservation> tourReservations, int id)
        {
            List<TourReservation> tourReservationsRemoved = tourReservations;
            tourReservationsRemoved.RemoveAll(t => t.Id == id);
            return tourReservationsRemoved;
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

        public List<TourReservation> GetAll()
        {
            return _tourReservations;
        }

        public List<int> GetTourIdsByUser(User user)
        {
            List<int> tourIds = new List<int>();

            foreach (TourReservation reservation in _tourReservations)
            {
                if (reservation.UserId == user.Id)
                {
                    tourIds.Add(reservation.TourId);
                }
            }
            return tourIds;
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
