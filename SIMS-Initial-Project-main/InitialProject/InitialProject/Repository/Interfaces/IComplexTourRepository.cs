using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Repository.Interfaces
{
    public interface IComplexTourRepository : ISubject
    {
        public int NextId();
        public ComplexTour Save(ComplexTour complexTour);
        public ComplexTour Update(ComplexTour complexTour);
        public void Delete(ComplexTour complexTour);
        public ComplexTour GetById(int id);
        public List<ComplexTour> GetAll();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
