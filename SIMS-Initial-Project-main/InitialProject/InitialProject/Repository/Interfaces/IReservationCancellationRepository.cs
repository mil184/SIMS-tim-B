using InitialProject.Model;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Repository.Interfaces
{
    public interface IReservationCancellationRepository : ISubject
    {
        public int NextId();
        public ReservationCancellation Save(ReservationCancellation reservationCancellation);
        public List<ReservationCancellation> GetAll();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
