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
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService()
        {
            _commentRepository = Injector.CreateInstance<ICommentRepository>();
        }

        public List<ForumComment> GetCommentsByForumId(int forumId)
        {
            List<ForumComment> comments = new List<ForumComment>();
            foreach (ForumComment comment in _commentRepository.GetAll())
            {
                if (comment.ForumId == forumId)
                {
                    comments.Add(comment);
                }
            }
            return comments;
        }

        public ForumComment GetById(int id)
        {
            foreach (ForumComment comment in _commentRepository.GetAll())
            {
                if (comment.Id == id)
                {
                    return comment;
                }
            }
            return null;
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
