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
        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;
        public ComplexTourService()
        {
            _complexTourRepository = Injector.CreateInstance<IComplexTourRepository>();
            _tourService = new TourService();
            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();
        }   

        public void AlterStatus(ComplexTour complexTour)
        {
            AlterAcceptedStatus(complexTour);
            AlterInvalidStatus(complexTour);
        }
 
        public void AlterAcceptedStatus(ComplexTour complexTour)
        {
            foreach(var id in complexTour.TourRequestIds)
            {
                if (_tourRequestService.GetById(id).Status != Resources.Enums.RequestStatus.accepted)
                {
                    return;
                }

                complexTour.Status = Resources.Enums.ComplexTourStatus.accepted;
            }
        }

        public void AlterInvalidStatus(ComplexTour complexTour)
        {
            TourRequest first = _tourRequestService.GetById(complexTour.TourRequestIds[0]);
            DateTime firstStartDate = first.StartTime;

            if (firstStartDate > firstStartDate.AddDays(2))
            {
                return;
            }

            foreach (var id in complexTour.TourRequestIds)
            {
                if (_tourRequestService.GetById(id).Status == Resources.Enums.RequestStatus.accepted)
                {
                    return;
                }

                complexTour.Status = Resources.Enums.ComplexTourStatus.invalid;
            }
        }

        public List<ComplexTour> GetAllByUser (User user)
        {
            List<ComplexTour> complexTours = new List<ComplexTour>();

            foreach(var tour in _complexTourRepository.GetAll())
            {
                if (tour.GuestId == user.Id)
                {
                    complexTours.Add(tour);
                }
            }

            return complexTours;
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
                if (!complexTour.AcceptedTourIdsByGuideIds.ContainsKey(user.Id) && complexTour.AvailableTourRequestIds.Count != 0) 
                {
                    tours.Add(complexTour);
                }
            }
            return tours;
        }
        public List<ComplexTour> FilterTours(string input, List<ComplexTour> tours)
        {

            List<ComplexTour> byCountry = new List<ComplexTour>();
            List<ComplexTour> byCity = new List<ComplexTour>();
            List<ComplexTour> byLanguage = new List<ComplexTour>();
            List<ComplexTour> byGuests = new List<ComplexTour>();

            foreach (var tour in tours)
            {
                  foreach (int id in tour.AvailableTourRequestIds)
                {
                    if (_locationService.GetById(_tourRequestService.GetById(id).LocationId).Country.ToLower().Replace(" ", "").Contains(input.ToLower().Replace(" ", "")))
                    {
                        byCountry.Add(tour);
                    }
                    if (_locationService.GetById(_tourRequestService.GetById(id).LocationId).City.ToLower().Replace(" ", "").Contains(input.ToLower().Replace(" ", "")))
                    {
                        byCity.Add(tour);
                    }
                    if (_tourRequestService.GetById(id).Language.ToLower().Replace(" ", "").Contains(input.ToLower().Replace(" ", "")))
                    {
                        byLanguage.Add(tour);
                    }
                    if (int.TryParse(input, out int x) && _tourRequestService.GetById(id).MaxGuests >= x)
                    {
                        byGuests.Add(tour);
                    }
                }
            }
            List<ComplexTour> combinedList = byCountry.Union(byCity).Union(byLanguage).Union(byGuests).Distinct().ToList();

            return combinedList;
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
