using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using InitialProject.ViewModel.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Service
{
    public class TourRequestService
    {
        public readonly ITourRequestRepository _tourRequestRepository;
        private readonly LocationService _locationService;
        private readonly TourService _tourService;
        public TourRequestService()
        {
            _tourRequestRepository = Injector.CreateInstance<ITourRequestRepository>();
            _locationService = new LocationService();
            _tourService = new TourService();
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
        public List<TourRequest> GetPendingRequests(User user)
        {
            List<TourRequest> pendingRequests = new List<TourRequest>();

            foreach (TourRequest request in GetAll())
            {
                if (request.Status == Resources.Enums.RequestStatus.pending)
                {
                    bool canBeAdded = true;
                    foreach (Tour tour in _tourService.GetGuideTours(user))
                    {
                        if (!(request.EndTime <= tour.StartTime || request.StartTime >= tour.StartTime.AddHours(tour.Duration)))
                        {
                            canBeAdded = false;
                            break; // Exit the inner loop as soon as an overlap is found
                        }
                    }
                    if (canBeAdded) 
                    {
                        pendingRequests.Add(request);
                    }
                }
            }
            return pendingRequests;
        }
        public List<TourRequest> GetByCity(User user,string city)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests(user))
            {
                if (_locationService.GetById(request.LocationId).City == city)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByYear(User guest, string year)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetAll())
            {
                if (request.GuestId == guest.Id)
                {
                    if (year == "All time")
                    {
                        requests = GetAll();
                    }
                    else if (request.StartTime.Year.ToString() == year)
                    {
                        requests.Add(request);
                    }
                }
            }

            return requests;
        }

        public int GetTotalGuestCountForYear(List<TourRequest> tourRequests)
        {
            int counter = 0;

            foreach(TourRequest tourRequest in tourRequests)
            {
                counter += tourRequest.MaxGuests;
            }

            return counter;
        }

        public List<TourRequest> GetAcceptedRequests(List<TourRequest> tourRequests)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in tourRequests)
            {
                if (request.Status.ToString() == "accepted")
                {
                    requests.Add(request);
                }
            }

            return requests;
        }

        public List<TourRequest> GetDeniedRequests(List<TourRequest> tourRequests)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in tourRequests)
            {
                if (request.Status.ToString() == "invalid")
                {
                    requests.Add(request);
                }
            }

            return requests;
        }

        public List<string> GetLanguages()
        {
            List<string> languages = new List<string>();

            foreach(TourRequest tourRequest in GetAll())
            {
               languages.Add(tourRequest.Language);
            }

            return languages.Distinct().ToList();
        }

        public int CountPerLanguage(string language)
        {
            int counter = 0;

            foreach(TourRequest tourRequest in GetAll())
            {
                if (tourRequest.Language == language)
                {
                    counter++;
                }
            }

            return counter;
        }

        public List<int> GetRequestCountForLanguage(List<string> languages)
        {
            List<int> counts = new List<int>();

            foreach(string language in languages)
            {
                counts.Add(CountPerLanguage(language));
            }

            return counts;
        }

        public List<TourRequest> GetByCountry(User user,string country)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests(user))
            {
                if (_locationService.GetById(request.LocationId).Country == country)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByMaxGuests(User user,int maxGuests)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests(user))
            {
                if (request.MaxGuests >= maxGuests)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByLanguage(User user, string language)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests(user))
            {
                if (request.Language.Replace(" ", "").ToLower().Contains(language.Replace(" ", "").ToLower()))
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByStartDate(User user, DateTime? startDate)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests(user))
            {
                if (request.StartTime >= startDate)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetByEndDate(User user, DateTime? endDate)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetPendingRequests(user))
            {
                if (request.EndTime <= endDate)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }

        public List<TourRequest> GetByAllParameters(User user,int year, int month, string city, string country, string language)
        {
            List<TourRequest> byLanguage = GetAll();
            if (language != null)
                byLanguage = GetByLanguage(user, language);

            List<TourRequest> byCountry = GetAll();
            if (country != null)
                byCountry = GetByCountry(user, country);

            List<TourRequest> byCity = GetAll();
            if (city != null)
                byCity = GetByCity(user, city);

            List<TourRequest> byYearMonth= GetAll();
            if (year != null && month != null) 
            {
                DateTime dateTimeYearMonthStart = new DateTime(year, month, 1, 0, 0, 0);
                DateTime dateTimeYearMonthEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

                byYearMonth = GetByStartDate(user,dateTimeYearMonthStart);
                byYearMonth.Intersect(GetByEndDate(user, dateTimeYearMonthEnd)).ToList();
            }

            List<TourRequest> byYear= GetAll();
            if (year != null && month == null) 
            {
                DateTime dateTimeYearStart = new DateTime(year,1, 1, 0, 0, 0);
                DateTime dateTimeYearEnd = new DateTime(year,12, DateTime.DaysInMonth(year, month), 23, 59, 59);

                byYear = GetByStartDate(user, dateTimeYearStart);
                byYear.Intersect(GetByEndDate(user, dateTimeYearEnd)).ToList();
            }

            List<TourRequest> result = byLanguage;
            result = result.Intersect(byCountry).ToList();
            result = result.Intersect(byCity).ToList();
            result = result.Intersect(byYearMonth).ToList();
            result = result.Intersect(byYear).ToList();

            return result;
        }

    }
}
