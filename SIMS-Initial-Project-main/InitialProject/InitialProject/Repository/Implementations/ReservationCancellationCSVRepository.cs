using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository.Implementations
{
    public class ReservationCancellationCSVRepository : IReservationCancellationRepository
    {
        private const string FilePath = "../../../Resources/Data/reservationCancellations.csv";
        private readonly Serializer<ReservationCancellation> _serializer;
        private List<ReservationCancellation> _reservationCancellations;
        private readonly List<IObserver> _observers;

        public ReservationCancellationCSVRepository()
        {
            _serializer = new Serializer<ReservationCancellation>();
            _reservationCancellations = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _reservationCancellations = _serializer.FromCSV(FilePath);
            if (_reservationCancellations.Count < 1)
            {
                return 1;
            }
            return _reservationCancellations.Max(c => c.Id) + 1;
        }

        public ReservationCancellation Save(ReservationCancellation reservationCancellation)
        {
            reservationCancellation.Id = NextId();

            _reservationCancellations = _serializer.FromCSV(FilePath);
            _reservationCancellations.Add(reservationCancellation);
            _serializer.ToCSV(FilePath, _reservationCancellations);
            NotifyObservers();

            return reservationCancellation;
        }

        public List<ReservationCancellation> GetAll()
        {
            return _reservationCancellations;
        }

        public ReservationCancellation GetById(int id)
        {
            _reservationCancellations = _serializer.FromCSV(FilePath);
            return _reservationCancellations.FirstOrDefault(u => u.Id == id);
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
