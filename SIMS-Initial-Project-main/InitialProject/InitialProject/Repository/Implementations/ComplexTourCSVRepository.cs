using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Repository.Implementations
{
    public class ComplexTourCSVRepository : IComplexTourRepository
    {
        private const string _filePath = "../../../Resources/Data/complexTours.csv";

        private readonly Serializer<ComplexTour> _serializer;

        private readonly List<IObserver> _observers;

        private List<ComplexTour> _complexTours; 

        public ComplexTourCSVRepository()
        {
            _serializer = new Serializer<ComplexTour>();
            _complexTours = _serializer.FromCSV(_filePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _complexTours = _serializer.FromCSV(_filePath);
            if (_complexTours.Count < 1)
            {
                return 1;
            }
            return _complexTours.Max(c => c.Id) + 1;
        }

        public ComplexTour Save(ComplexTour complexTour)
        {
            complexTour.Id = NextId();
            _complexTours = _serializer.FromCSV(_filePath);
            _complexTours.Add(complexTour);
            _serializer.ToCSV(_filePath, _complexTours);
            NotifyObservers();

            return complexTour;
        }

        public ComplexTour Update(ComplexTour complexTour)
        {
            _complexTours = _serializer.FromCSV(_filePath);
            ComplexTour current = _complexTours.Find(c => c.Id == complexTour.Id);
            int index = _complexTours.IndexOf(current);
            _complexTours.Remove(current);
            _complexTours.Insert(index, complexTour);
            _serializer.ToCSV(_filePath, _complexTours);
            NotifyObservers();
            return complexTour;
        }
        public void Delete(ComplexTour complexTour)
        {
            _complexTours = _serializer.FromCSV(_filePath);
            ComplexTour founded = _complexTours.Find(c => c.Id == complexTour.Id);
            _complexTours.Remove(founded);
            _serializer.ToCSV(_filePath, _complexTours);
        }
        public ComplexTour GetById(int id)
        {
            _complexTours = _serializer.FromCSV(_filePath);
            return _complexTours.Find(c => c.Id == id);
        }

        public List<ComplexTour> GetAll()
        {
            return _complexTours;
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
