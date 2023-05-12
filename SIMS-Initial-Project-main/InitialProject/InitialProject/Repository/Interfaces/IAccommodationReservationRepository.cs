using InitialProject.Model;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Repository.Interfaces
{
    public interface IAccommodationReservationRepository : ISubject
    {
        public int NextId();
        public AccommodationReservation Save(AccommodationReservation accommodationReservation);
        public List<AccommodationReservation> GetAll();
        public AccommodationReservation GetById(int id);
        public AccommodationReservation Update(AccommodationReservation accommodationReservation);
        public void Remove(AccommodationReservation reservation);
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
