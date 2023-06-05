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
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService()
        {
            _commentRepository = Injector.CreateInstance<ICommentRepository>();
        }

        public ForumComment Save(ForumComment forumComment)
        {
            return _commentRepository.Save(forumComment);
        }

        public ForumComment Update(ForumComment forumComment)
        {
            return _commentRepository.Update(forumComment);
        }

        public List<ForumComment> GetAll()
        {
            return _commentRepository.GetAll();
        }

        public void Subscribe(IObserver observer)
        {
            _commentRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _commentRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _commentRepository.NotifyObservers();
        }
    }
}
