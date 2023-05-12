using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class TourRequestService
    {
        public readonly ITourRequestRepository _tourRequestRepository;
        private readonly LocationService _locationService;

        public TourRequestService()
        {
            _tourRequestRepository = Injector.CreateInstance<ITourRequestRepository>();
            _locationService = new LocationService();
        }

        public TourRequest GetById(int id)
        {
            return _tourRequestRepository.GetById(id);
        }

        public void Update(TourRequest tourRequest)
        {
            _tourRequestRepository.Update(tourRequest);
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            return _tourRequestRepository.Save(tourRequest);
        }

        internal List<TourRequest> GetAll()
        {
            return _tourRequestRepository.GetAll();
        }

        public void Subscribe(IObserver observer)
        {
            _tourRequestRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _tourRequestRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _tourRequestRepository.NotifyObservers();
        }
        public List<TourRequest> GetPendingRequests() 
        {
            List<TourRequest> pendingRequests = new List<TourRequest>();

            foreach(TourRequest request in GetAll()) 
            {
                if(request.Status == Resources.Enums.RequestStatus.pending) 
                {
                    pendingRequests.Add(request);
                }
            }
            return pendingRequests;
        }
        public List<TourRequest> GetByCity(string city)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests())
            {
                if (_locationService.GetById(request.LocationId).City == city)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByCountry(string country)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests())
            {
                if (_locationService.GetById(request.LocationId).Country == country)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByMaxGuests(int maxGuests)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests())
            {
                if (request.MaxGuests >= maxGuests)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByLanguage(string language)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests())
            {
                if (request.Language.Replace(" ", "").ToLower().Contains(language.Replace(" ", "").ToLower()))
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByStartDate(DateTime? startDate)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests())
            {
                if (request.StartTime >= startDate)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByEndDate(DateTime? endDate)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests())
            {
                if (request.EndTime <= endDate)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }

    }
}
