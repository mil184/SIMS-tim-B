using InitialProject.Model;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Repository.Interfaces
{
    public interface ITourRequestRepository : ISubject
    {
        public int NextId();
        public TourRequest Save(TourRequest tourRequest);
        public TourRequest Update(TourRequest tourRequest);
        public TourRequest GetById(int id);
        public List<TourRequest> GetAll();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
