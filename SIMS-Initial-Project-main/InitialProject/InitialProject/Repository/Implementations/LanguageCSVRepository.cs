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
    internal class LanguageCSVRepository : ILanguageRepository
    {
        private const string FilePath = "../../../Resources/Data/languages.csv";

        private readonly Serializer<Language> _serializer;

        private List<Language> _languages;

        private readonly List<IObserver> _observers;
        public LanguageCSVRepository()
        {
            _serializer = new Serializer<Language>();
            _languages = _serializer.FromCSV(FilePath);
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            _languages = _serializer.FromCSV(FilePath);
            if (_languages.Count < 1)
            {
                return 1;
            }
            return _languages.Max(c => c.Id) + 1;
        }

        public Language Save(Language language)
        {
            language.Id = NextId();

            _languages = _serializer.FromCSV(FilePath);
            _languages.Add(language);
            _serializer.ToCSV(FilePath, _languages);
            NotifyObservers();

            return language;
        }
        public Language GetById(int id)
        {
            _languages = _serializer.FromCSV(FilePath);
            return _languages.Find(c => c.Id == id);
        }

        public List<Language> GetAll()
        {
            return _languages;
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
