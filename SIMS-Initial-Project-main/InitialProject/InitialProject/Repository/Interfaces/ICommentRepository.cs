using InitialProject.Model;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository.Interfaces
{
    public interface ICommentRepository: ISubject
    {
        public int NextId();
        public ForumComment Save(ForumComment forumComment);
        public ForumComment Update(ForumComment forumComment);
        public List<ForumComment> GetAll();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
