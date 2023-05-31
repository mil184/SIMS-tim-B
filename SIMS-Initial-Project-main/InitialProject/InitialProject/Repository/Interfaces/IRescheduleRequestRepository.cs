using InitialProject.Model;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Repository.Interfaces
{
    public interface IRescheduleRequestRepository : ISubject
    {
        public int NextId();
        public RescheduleRequest Save(RescheduleRequest request);
        public RescheduleRequest Update(RescheduleRequest request);
        public List<RescheduleRequest> GetAll();
        public RescheduleRequest GetById(int id);
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
