using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository.Implementations
{
    public class AccommodationRenovationCSVRepository : IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRenovations.csv";
        private readonly Serializer<AccommodationRenovation> _serializer;
        private List<AccommodationRenovation> _accommodationRenovations;
        private readonly List<IObserver> _observers;

        public AccommodationRenovationCSVRepository()
        {
            _serializer = new Serializer<AccommodationRenovation>();
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            if (_accommodationRenovations.Count < 1)
            {
                return 1;
            }
            return _accommodationRenovations.Max(c => c.Id) + 1;
        }

        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovation.Id = NextId();

            _accommodationRenovations = _serializer.FromCSV(FilePath);
            _accommodationRenovations.Add(accommodationRenovation);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
            NotifyObservers();

            return accommodationRenovation;
        }

        public void Remove(int id)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            _accommodationRenovations.RemoveAll(item => item.Id == id);
            _serializer.ToCSV(FilePath, _accommodationRenovations);
            NotifyObservers();
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _accommodationRenovations;
        }

        public AccommodationRenovation GetById(int id)
        {
            _accommodationRenovations = _serializer.FromCSV(FilePath);
            return _accommodationRenovations.FirstOrDefault(u => u.Id == id);
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
