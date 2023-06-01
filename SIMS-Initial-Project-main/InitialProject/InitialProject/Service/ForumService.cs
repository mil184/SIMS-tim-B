using InitialProject.Model;
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
    public class ForumService
    {
        private readonly IForumRepository _forumRepository;

        public ForumService()
        {
            _forumRepository = Injector.CreateInstance<IForumRepository>();
        }

        public Forum Save(Forum forum)
        {
            return _forumRepository.Save(forum);
        }

        public List<Forum> GetAll()
        {
            return _forumRepository.GetAll();
        }

        public void Subscribe(IObserver observer)
        {
            _forumRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _forumRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _forumRepository.NotifyObservers();
        }
    }
}
