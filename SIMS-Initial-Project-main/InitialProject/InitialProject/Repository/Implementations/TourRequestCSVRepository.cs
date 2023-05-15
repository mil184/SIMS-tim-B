using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository.Implementations
{
    public class TourRequestCSVRepository : ITourRequestRepository
    {
        private const string _filePath = "../../../Resources/Data/tourRequests.csv";

        private readonly Serializer<TourRequest> _serializer;

        private readonly List<IObserver> _observers;

        private List<TourRequest> _tourRequests;

        public TourRequestCSVRepository()
        {
            _serializer = new Serializer<TourRequest>();
            _tourRequests = _serializer.FromCSV(_filePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _tourRequests = _serializer.FromCSV(_filePath);
            if (_tourRequests.Count < 1)
            {
                return 1;
            }
            return _tourRequests.Max(c => c.Id) + 1;
        }   

        public TourRequest Save(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            _tourRequests = _serializer.FromCSV(_filePath);
            _tourRequests.Add(tourRequest);
            _serializer.ToCSV(_filePath, _tourRequests);
            NotifyObservers();

            return tourRequest;
        }

        public TourRequest Update(TourRequest tourRequest)
        {
            _tourRequests = _serializer.FromCSV(_filePath);
            TourRequest current = _tourRequests.Find(c => c.Id == tourRequest.Id);
            int index = _tourRequests.IndexOf(current);
            _tourRequests.Remove(current);
            _tourRequests.Insert(index, tourRequest);
            _serializer.ToCSV(_filePath, _tourRequests);
            NotifyObservers();
            return tourRequest;
        }

        public TourRequest GetById(int id)
        {
            _tourRequests = _serializer.FromCSV(_filePath);
            return _tourRequests.Find(c => c.Id == id);
        }

        public List<TourRequest> GetAll()
        {
            return _tourRequests;
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
