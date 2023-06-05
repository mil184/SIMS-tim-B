using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository.Implementations
{
    public class ForumCSVRepository: IForumRepository
    {
        private const string FilePath = "../../../Resources/Data/forums.csv";
        private readonly Serializer<Forum> _serializer;
        private List<Forum> _forums;
        private readonly List<IObserver> _observers;

        public ForumCSVRepository()
        {
            _serializer = new Serializer<Forum>();
            _forums = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _forums = _serializer.FromCSV(FilePath);
            if (_forums.Count < 1)
            {
                return 1;
            }
            return _forums.Max(c => c.Id) + 1;
        }

        public Forum Save(Forum forum)
        {
            forum.Id = NextId();

            _forums = _serializer.FromCSV(FilePath);
            _forums.Add(forum);
            _serializer.ToCSV(FilePath, _forums);
            NotifyObservers();

            return forum;
        }

        public Forum Update(Forum forum)
        {
            _forums = _serializer.FromCSV(FilePath);
            Forum current = _forums.Find(c => c.Id == forum.Id);
            int index = _forums.IndexOf(current);
            _forums.Remove(current);
            _forums.Insert(index, forum);
            _serializer.ToCSV(FilePath, _forums);
            NotifyObservers();
            return forum;
        }

        public List<Forum> GetAll()
        {
            return _forums;
        }

        public Forum GetById(int id)
        {
            _forums = _serializer.FromCSV(FilePath);
            return _forums.FirstOrDefault(f => f.Id == id);
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
