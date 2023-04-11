using InitialProject.Model;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository.Interfaces
{
    public interface ICheckpointRepository : ISubject
    {
        public int NextId();
        public Checkpoint GetById(int id);
        public List<Checkpoint> GetAll();
        public Checkpoint Save(Checkpoint checkpoint);
        public Checkpoint Update(Checkpoint checkpoint);
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();

    }
}
