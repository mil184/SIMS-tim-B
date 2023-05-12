using InitialProject.Model;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Repository.Interfaces
{
    public interface IAccommodationRepository : ISubject
    {
        public int NextId();
        public Accommodation Save(Accommodation accommodation);
        public Accommodation GetById(int id);
        public List<Accommodation> GetAll();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
