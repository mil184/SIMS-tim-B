using InitialProject.Model;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Repository.Interfaces
{
    public interface IGuestReviewRepository : ISubject
    {
        public int NextId();
        public GuestReview Save(GuestReview guestReview);
        public List<GuestReview> GetAll();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
