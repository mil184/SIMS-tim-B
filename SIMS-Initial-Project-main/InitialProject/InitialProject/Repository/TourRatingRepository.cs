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
    public class TourRatingRepository : ISubject
    {
        private const string FilePath = "../../../Resources/Data/tourRatings.csv";

        private readonly Serializer<TourRating> _serializer;

        private List<TourRating> _tourRatings;

        private readonly List<IObserver> _observers;

        public TourRatingRepository()
        {
            _serializer = new Serializer<TourRating>();
            _tourRatings = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
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

        public int NextId()
        {
            _tourRatings = _serializer.FromCSV(FilePath);
            if (_tourRatings.Count < 1)
            {
                return 1;
            }
            return _tourRatings.Max(c => c.Id) + 1;
        }

        public TourRating Save(TourRating tourRating)
        {
            tourRating.Id = NextId();

            _tourRatings = _serializer.FromCSV(FilePath);
            _tourRatings.Add(tourRating);
            _serializer.ToCSV(FilePath, _tourRatings);
            NotifyObservers();

            return tourRating;
        }

        public TourRating Update(TourRating tourRating)
        {
            _tourRatings = _serializer.FromCSV(FilePath);
            TourRating current = _tourRatings.Find(c => c.Id == tourRating.Id);
            int index = _tourRatings.IndexOf(current);
            _tourRatings.Remove(current);
            _tourRatings.Insert(index, tourRating);
            _serializer.ToCSV(FilePath, _tourRatings);
            NotifyObservers();
            return tourRating;
        }

        public List<TourRating> GetAll()
        {
            return _tourRatings;
        }

        public TourRating GetById(int id)
        {
            _tourRatings = _serializer.FromCSV(FilePath);
            return _tourRatings.Find(c => c.Id == id);
        }
    }
}
