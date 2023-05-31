using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository.Interfaces
{
    public interface IUserRepository: ISubject
    {
        public User GetById(int id);
        public User Update(User user);
        public User Save(User user);
        public int NextId();
        public List<User> GetAll();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
        
    }
}
