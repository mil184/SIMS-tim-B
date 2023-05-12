using InitialProject.Model;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Repository.Interfaces
{
    public interface IAccommodationRatingRepository : ISubject
    {
        public int NextId();
        public AccommodationRatings Save(AccommodationRatings accommodationRatings);
        public AccommodationRatings Update(AccommodationRatings accommodationRatings);
        public List<AccommodationRatings> GetAll();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
