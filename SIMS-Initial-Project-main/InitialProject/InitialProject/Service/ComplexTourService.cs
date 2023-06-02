using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Repository;
using InitialProject.Resources.Injector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System.Xml.Linq;

namespace InitialProject.Service
{
    public class ComplexTourService : ISubject
    {
        private readonly IComplexTourRepository _complexTourRepository;
        private readonly TourService _tourService;

        public ComplexTourService()
        {
            _complexTourRepository = Injector.CreateInstance<IComplexTourRepository>();
            _tourService = new TourService();
        }   
        public List<Dictionary<int, int>> GetDictionaries() 
        {
            List<Dictionary<int, int>> dictionaries = new List<Dictionary<int, int>>();

            foreach (ComplexTour complexTour in GetAll()) 
            {
                dictionaries.Add(complexTour.AcceptedTourIdsByGuideIds);
            }
            return dictionaries;
        }  

        public List<Tour> GetToursByGuide(User user) 
        {
            List<Tour> tours = new List<Tour>();

            foreach (Dictionary<int, int> dictionary in GetDictionaries()) 
            {
                if (dictionary.ContainsKey(user.Id)) 
                {
                    tours.Add(_tourService.GetById(dictionary.GetValueOrDefault(user.Id)));
                }
            }
            return tours;
        }

        public int GetNumberOfAvailableTours(ComplexTour complexTour) 
        {
            return complexTour.AvailableTourRequestIds.Count;
        }
        public ComplexTour GetById(int id)
        {
            return _complexTourRepository.GetById(id);
        }

        public void Subscribe(IObserver observer)
        {
            _complexTourRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _complexTourRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _complexTourRepository.NotifyObservers();
        }

        public void Update(ComplexTour complexTour)
        {
            _complexTourRepository.Update(complexTour);
        }

        public ComplexTour Save(ComplexTour complexTour)
        {
            return _complexTourRepository.Save(complexTour);
        }

        public List<ComplexTour> GetAll()
        {
            return _complexTourRepository.GetAll();
        }
    }
}
