using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository.Implementations
{
    public class RenovationRecommendationCSVRepository: IRenovationRecommendationRepository
    {
        private const string FilePath = "../../../Resources/Data/renovationRecommendations.csv";

        private readonly Serializer<RenovationRecommendation> _serializer;

        private List<RenovationRecommendation> _renovationRecommendations;

        private readonly List<IObserver> _observers;

        public RenovationRecommendationCSVRepository()
        {
            _serializer = new Serializer<RenovationRecommendation>();
            _renovationRecommendations = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _renovationRecommendations = _serializer.FromCSV(FilePath);
            if (_renovationRecommendations.Count < 1)
            {
                return 1;
            }
            return _renovationRecommendations.Max(c => c.Id) + 1;
        }

        public RenovationRecommendation Save(RenovationRecommendation renovationRecommendations)
        {
            renovationRecommendations.Id = NextId();

            _renovationRecommendations = _serializer.FromCSV(FilePath);
            _renovationRecommendations.Add(renovationRecommendations);
            _serializer.ToCSV(FilePath, _renovationRecommendations);
            NotifyObservers();

            return renovationRecommendations;
        }

        public RenovationRecommendation Update(RenovationRecommendation renovationRecommendations)
        {
            _renovationRecommendations = _serializer.FromCSV(FilePath);
            RenovationRecommendation current = _renovationRecommendations.Find(c => c.Id == renovationRecommendations.Id);
            int index = _renovationRecommendations.IndexOf(current);
            _renovationRecommendations.Remove(current);
            _renovationRecommendations.Insert(index, renovationRecommendations);
            _serializer.ToCSV(FilePath, _renovationRecommendations);
            NotifyObservers();
            return renovationRecommendations;
        }

        public RenovationRecommendation GetById(int id)
        {
            _renovationRecommendations = _serializer.FromCSV(FilePath);
            return _renovationRecommendations.FirstOrDefault(u => u.Id == id);
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _renovationRecommendations;
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
