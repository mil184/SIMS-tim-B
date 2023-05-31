using InitialProject.Model;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Repository.Interfaces
{
    public interface IAccommodationRenovationRepository : ISubject
    {
        public int NextId();
        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation);
        public void Remove(int id);
        public List<AccommodationRenovation> GetAll();
        public AccommodationRenovation GetById(int id);
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
