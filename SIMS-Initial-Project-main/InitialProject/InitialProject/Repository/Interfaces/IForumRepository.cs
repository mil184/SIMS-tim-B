using InitialProject.Model;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository.Interfaces
{
    public interface IForumRepository: ISubject
    {
        public int NextId();
        public Forum Save(Forum forum);
        public List<Forum> GetAll();
        public Forum Update(Forum forum);
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
