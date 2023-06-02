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
 

        public List<DateOnly> GetAvailableTimeSlots(User user, ComplexTour complexTour, TourRequest tourRequest)
        {
            List<DateOnly> usedDatesByToursInComplexTour = new List<DateOnly>();

            List<DateOnly> usedDatesByToursByUser = new List<DateOnly>();

            List<DateOnly> tourRequestSlots = new List<DateOnly>();

            foreach (int id in complexTour.AcceptedTourIdsByGuideIds.Values)
            {
                Tour tour = _tourService.GetById(id);
                DateOnly date = new DateOnly(tour.StartTime.Year, tour.StartTime.Month, tour.StartTime.Day);
                usedDatesByToursInComplexTour.Add(date);
            }

            for (DateTime iterator = tourRequest.StartTime; iterator <= tourRequest.EndTime; iterator = iterator.AddDays(1))
            {
                tourRequestSlots.Add(new DateOnly(iterator.Year, iterator.Month, iterator.Day));
            }

            usedDatesByToursByUser = _tourService.GetFilledTimeSlots(user);

            tourRequestSlots.RemoveAll(date => usedDatesByToursInComplexTour.Contains(date));

            tourRequestSlots.RemoveAll(date => usedDatesByToursByUser.Contains(date));

            return tourRequestSlots;
        }
        public List<ComplexTour> GetAvailableComplexTours(User user) 
        {
            List<ComplexTour> tours = new List<ComplexTour>();

            foreach(ComplexTour complexTour in GetAll()) 
            {
                if (!complexTour.AcceptedTourIdsByGuideIds.ContainsKey(user.Id)) 
                {
                    tours.Add(complexTour);
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
