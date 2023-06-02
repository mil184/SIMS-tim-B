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
    public class CommentCSVRepository: ICommentRepository
    {
        private const string FilePath = "../../../Resources/Data/forumComments.csv";
        private readonly Serializer<ForumComment> _serializer;
        private List<ForumComment> _comments;
        private readonly List<IObserver> _observers;

        public CommentCSVRepository()
        {
            _serializer = new Serializer<ForumComment>();
            _comments = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _comments = _serializer.FromCSV(FilePath);
            if (_comments.Count < 1)
            {
                return 1;
            }
            return _comments.Max(c => c.Id) + 1;
        }

        public ForumComment Save(ForumComment forumComment)
        {
            forumComment.Id = NextId();

            _comments = _serializer.FromCSV(FilePath);
            _comments.Add(forumComment);
            _serializer.ToCSV(FilePath, _comments);
            NotifyObservers();

            return forumComment;
        }

        public ForumComment Update(ForumComment forumComment)
        {
            _comments = _serializer.FromCSV(FilePath);
            ForumComment current = _comments.Find(c => c.Id == forumComment.Id);
            int index = _comments.IndexOf(current);
            _comments.Remove(current);
            _comments.Insert(index, forumComment);
            _serializer.ToCSV(FilePath, _comments);
            NotifyObservers();
            return forumComment;
        }

        public List<ForumComment> GetAll()
        {
            return _comments;
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
