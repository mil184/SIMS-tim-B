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
    public class AccommodationRatingsRepository: ISubject
    {
        private const string FilePath = "../../../Resources/Data/accommodationRatings.csv";

        private readonly Serializer<AccommodationRatings> _serializer;

        private List<AccommodationRatings> _accommodationRatings;

        private readonly List<IObserver> _observers;

        public AccommodationRatingsRepository()
        {
            _serializer = new Serializer<AccommodationRatings>();
            _accommodationRatings = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            if (_accommodationRatings.Count < 1)
            {
                return 1;
            }
            return _accommodationRatings.Max(c => c.Id) + 1;
        }

        public AccommodationRatings Save(AccommodationRatings accommodationRatings)
        {
            accommodationRatings.Id = NextId();

            _accommodationRatings = _serializer.FromCSV(FilePath);
            _accommodationRatings.Add(accommodationRatings);
            _serializer.ToCSV(FilePath, _accommodationRatings);
            NotifyObservers();

            return accommodationRatings;
        }

        public List<AccommodationRatings> GetAll()
        {
            return _accommodationRatings;
        }

        public AccommodationRatings GetById(int id)
        {
            _accommodationRatings = _serializer.FromCSV(FilePath);
            return _accommodationRatings.FirstOrDefault(u => u.Id == id);
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
