using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public Forum GetById(int id)
        {
            foreach (Forum forum in _forumRepository.GetAll())
            {
                if (forum.Id == id)
                {
                    return forum;
                }
            }
            return null;
        }

        public Forum Save(Forum forum)
        {
            return _forumRepository.Save(forum);
        }

        public void Update(Forum forum)
        {
            _forumRepository.Update(forum);
        }

        public List<Forum> GetAll()
        {
            return _forumRepository.GetAll();
        }

        public Forum GetForumById(int forumId)
        {
            foreach (Forum forum in _forumRepository.GetAll())
            {
                if (forum.Id == forumId)
                {
                    return forum;
                }
            }
            return null;
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
