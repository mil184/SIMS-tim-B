using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class CheckpointService : ISubject
    {
        private readonly ICheckpointRepository _checkpointRepository;

        public CheckpointService() 
        {
            _checkpointRepository = Injector.CreateInstance<ICheckpointRepository>();
        }

        public Checkpoint Save(Checkpoint checkpoint) 
        {
            return _checkpointRepository.Save(checkpoint);
        }

        public Checkpoint Update(Checkpoint checkpoint)
        {
            return _checkpointRepository.Update(checkpoint);
        }
        public List<Checkpoint> GetByTour(Tour tour)    
        {
            return GetAll().Where(c => c.TourId == tour.Id).ToList();
        }
        public void NotifyObservers()
        {
            _checkpointRepository.NotifyObservers();
        }
        public void Subscribe(IObserver observer)
        {
            _checkpointRepository.Subscribe(observer);
        }
        public void Unsubscribe(IObserver observer)
        {
            _checkpointRepository.Unsubscribe(observer);
        }
        public List<Checkpoint> GetAll() 
        {
            return _checkpointRepository.GetAll();
        }
        public Checkpoint GetById(int id) 
        {
            return _checkpointRepository.GetById(id);
        }
    }
}
