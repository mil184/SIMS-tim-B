using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using InitialProject.View.Guest2;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Windows;

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

        public TourRequest GetSameDetailsTourRequest(User user)
        {
            List<TourRequest> acceptedOtherGuestRequests = GetStatusRequests(GetOtherGuestRequests(user), Resources.Enums.RequestStatus.accepted);

            foreach (TourRequest guestRequest in GetInvalidandPendingRequests(user))
            {
                foreach (TourRequest otherGuestRequest in acceptedOtherGuestRequests)
                {
                    if (guestRequest.LocationId == otherGuestRequest.LocationId || guestRequest.Language == otherGuestRequest.Language)
                    {
                        return guestRequest;
                    }
                }
            }

            return null;
        }
        
        public List<string> GetInvalidOrPendingLanguageRequests(User user) 
        {
            List<string> languages = new List<string>();

            foreach (TourRequest request in GetInvalidandPendingRequests(user)) 
            {
                if (!languages.Contains(request.Language)) 
                {
                    languages.Add(request.Language);
                }
            }
            return languages;
        }
        public List<int> GetInvalidOrPendingLocationRequests(User user)
        {
            List<int> locationIds = new List<int>();

            foreach (TourRequest request in GetInvalidandPendingRequests(user))
            {
                if (!locationIds.Contains(request.LocationId))
                {
                    locationIds.Add(request.LocationId);
                }
            }
            return locationIds;
        }
        public List<TourRequest> CheckForOthersAcceptedLanguage(User user) 
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetOtherGuestRequests(user)) 
            {
                if (GetInvalidOrPendingLanguageRequests(user).Contains(request.Language) && request.Status == Resources.Enums.RequestStatus.accepted) 
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> CheckForOthersAcceptedLocation(User user)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in GetOtherGuestRequests(user))
            {
                if (GetInvalidOrPendingLocationRequests(user).Contains(request.LocationId) && request.Status == Resources.Enums.RequestStatus.accepted)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<TourRequest> GetInvalidandPendingRequests(User user)
        {
            List<TourRequest> requests = GetStatusRequests(GetGuestRequests(user), Resources.Enums.RequestStatus.pending);

            foreach (TourRequest request in GetAll())
            {
                if (request.Status == InitialProject.Resources.Enums.RequestStatus.invalid)
                {
                    requests.Add(request);
                }
            }

            return requests;
        }

        public List<TourRequest> GetStatusRequests (List<TourRequest> tourRequests, InitialProject.Resources.Enums.RequestStatus requestStatus)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (TourRequest request in tourRequests)
            {
                if (request.Status == requestStatus)
                {
                    requests.Add(request);
                }
            }

            return requests;
        }
        public void UpdateInvalidRequests() 
        {
            foreach(TourRequest request in GetAll()) 
            {
                if(request.StartTime.AddDays(-2) < DateTime.Now && request.Status == Resources.Enums.RequestStatus.pending) 
                {
                    request.Status = Resources.Enums.RequestStatus.invalid;
                    Update(request);
                }
            }
        }
        public List<TourRequest> GetGuestRequests(User user)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach(var request in _tourRequestRepository.GetAll())
            {
                if (request.GuestId == user.Id)
                {
                    requests.Add(request);
                }
            }

            return requests;
        }

        public List<TourRequest> GetOtherGuestRequests(User user)
        {
            List<TourRequest> requests = new List<TourRequest>();

            foreach (var request in _tourRequestRepository.GetAll())
            {
                if (request.GuestId != user.Id)
                {
                    requests.Add(request);
                }
            }

            return requests;
        }

        public List<TourRequest> GetPendingRequests(User user)
        {
            List<TourRequest> pendingRequests = new List<TourRequest>();

            foreach (TourRequest request in GetAll())
            {
                if (request.Status == Resources.Enums.RequestStatus.pending && !request.IsInComplexTour)
                {
                    bool canBeAdded = true;
                    foreach (Tour tour in _tourService.GetAllUnabortedGuideTours(user))
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
        public List<TourRequest> Search(string input, List<TourRequest> requests)
        {

            List<TourRequest> byCountry = new List<TourRequest>();

            foreach (var request in requests)
            {
                if (_locationService.GetById(request.LocationId).Country.ToLower().Replace(" ", "").Contains(input.ToLower().Replace(" ", "")))
                {
                    byCountry.Add(request);
                }
            }
            List<TourRequest> byCity = new List<TourRequest>();

            foreach (var request in requests)
            {
                if (_locationService.GetById(request.LocationId).City.ToLower().Replace(" ", "").Contains(input.ToLower().Replace(" ", "")))
                {
                    byCity.Add(request);
                }
            }
            List<TourRequest> byLanguage = new List<TourRequest>();

            foreach (var request in requests)
            {
                if (request.Language.ToLower().Replace(" ", "").Contains(input.ToLower().Replace(" ", "")))
                {
                    byLanguage.Add(request);
                }
            }

            List<TourRequest> byGuests = new List<TourRequest>();
            if (int.TryParse(input, out int x))
            {
                foreach (var request in requests)
                {
                    if (request.MaxGuests >= x)
                    {
                        byLanguage.Add(request);
                    }
                }

            }
    
            List<TourRequest> combinedList = byCountry.Union(byCity).Union(byLanguage).Union(byGuests).Distinct().ToList();

            return combinedList;
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

        //public List<TourRequest> GetAcceptedRequests(List<TourRequest> tourRequests)
        //{
        //    List<TourRequest> requests = new List<TourRequest>();

        //    foreach (TourRequest request in tourRequests)
        //    {
        //        if (request.Status.ToString() == "accepted")
        //        {
        //            requests.Add(request);
        //        }
        //    }

        //    return requests;
        //}

        //public List<TourRequest> GetDeniedRequests(List<TourRequest> tourRequests)
        //{
        //    List<TourRequest> requests = new List<TourRequest>();

        //    foreach (TourRequest request in tourRequests)
        //    {
        //        if (request.Status.ToString() == "invalid")
        //        {
        //            requests.Add(request);
        //        }
        //    }

        //    return requests;
        //}

        public List<string> GetLanguages()
        {
            List<string> languages = new List<string>();

            foreach(TourRequest tourRequest in GetAll())
            {
               languages.Add(tourRequest.Language);
            }

            return languages.Distinct().ToList();
        }

        public List<string> GetRequestedCities()
        {
            List<string> cities = new List<string>();

            foreach (TourRequest tourRequest in GetAll())
            {
                string city = _locationService.GetCityById(tourRequest.LocationId);
                cities.Add(city);
            }

            return cities.Distinct().ToList();
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

        public int CountPerCity(string city)
        {
            int counter = 0;

            foreach (TourRequest tourRequest in GetAll())
            {
                string currentCity = _locationService.GetCityById(tourRequest.LocationId);
                if (currentCity == city)
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

        public List<int> GetRequestCountForCity(List<string> cities)
        {
            List<int> counts = new List<int>();

            foreach (string city in cities)
            {
                counts.Add(CountPerCity(city));
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

        public List<TourRequest> Filter(RequestFilterParameters parameters, User user)
        {
            List<TourRequest> requests = new List<TourRequest>();
            if (user == null) 
            {
                requests = HandleAllRequests(parameters);
                return requests;
            }
            else
            {
                requests = GetPendingRequests(user);
            }

            if (!string.IsNullOrEmpty(parameters.Country)) 
            {
               requests = requests.Intersect(GetByCountry(user, parameters.Country)).ToList();
            }

            if (!string.IsNullOrEmpty(parameters.City))
            {
                requests = requests.Intersect(GetByCity(user, parameters.City)).ToList();
            }

            if (!string.IsNullOrEmpty(parameters.Language))
            {
                requests = requests.Intersect(GetByLanguage(user, parameters.Language)).ToList();
            }
            if (parameters.MaxGuests != null)
            {
                requests = requests.Intersect(GetByMaxGuests(user, parameters.MaxGuests.Value)).ToList();
            }
            if (parameters.StartDate != null)
            {
                requests = requests.Intersect(GetByStartDate(user, parameters.StartDate.Value)).ToList();
            }
            if (parameters.EndDate != null)
            {
                requests = requests.Intersect(GetByEndDate(user, parameters.EndDate.Value)).ToList();
            }

            return requests; 
        }
        private List<TourRequest> HandleAllRequests(RequestFilterParameters parameters) 
        {
            List<TourRequest> requests = GetAll();

            if (!string.IsNullOrEmpty(parameters.Country))
            {
                requests = requests.Intersect(requests.Where(r => _locationService.GetById(r.LocationId).Country == parameters.Country)).ToList();
            }

            if (!string.IsNullOrEmpty(parameters.City))
            {
                requests = requests.Intersect(requests.Where(r => _locationService.GetById(r.LocationId).City == parameters.Country)).ToList();
            }

            if (!string.IsNullOrEmpty(parameters.Language))
            {
                requests = requests.Intersect(requests.Where(r => r.Language.Replace(" ", "").ToLower() == parameters.Language.Replace(" ", "").ToLower())).ToList();
            }

            if (parameters.StartDate != null)
            {
                requests = requests.Intersect(requests.Where(r => r.StartTime >= parameters.StartDate)).ToList();
            }   
            if (parameters.EndDate != null)
            {
                requests = requests.Intersect(requests.Where(r => r.EndTime <= parameters.EndDate)).ToList();
            }

            return requests;
        }
        public List<string> GetAllLanguages() 
        {
            List<string> languages = new List<string>();

            RequestFilterParameters parameters = new RequestFilterParameters();

            foreach (TourRequest request in GetAll()) 
            {
                if (!languages.Contains(request.Language)) 
                {
                    languages.Add(request.Language);
                }
            }
            return languages;
        }
        public string GetMostRequestedLanguage()
        {
            int maxValue = 0;
            string returnLanguage = "";
            foreach (string language in GetAllLanguages()) 
            {
                RequestFilterParameters parameters = new RequestFilterParameters(null,null, language, null, null,null);

                int count = Filter(parameters,null).Count;

                if (count > maxValue)
                {
                    maxValue = count;
                    returnLanguage = language;
                }
            }
            return returnLanguage;
        }
        public string GetMostRequestedCity()
        {
            int maxValue = 0;
            string returnCity = "";

            foreach (string city in _locationService.GetCities())
            {
                string country = _locationService.GetCountryByCity(city);

                RequestFilterParameters parameters = new RequestFilterParameters(null, city, null, null, null, null);

                int count = Filter(parameters, null).Count;

                if (count > maxValue)
                {
                    maxValue = count;
                    returnCity = city;
                }
            }
            return returnCity;
        }
        public string GetMostRequestedCountry()
        {
            int maxValue = 0;
            string returnCountry = "";
            foreach (string country in _locationService.GetCountries())
            {
                RequestFilterParameters parameters = new RequestFilterParameters(country, null, null, null, null, null);

                int count = Filter(parameters, null).Count;

                if (count > maxValue)
                {
                    maxValue = count;
                    returnCountry = country;
                }
            }
            return returnCountry;
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
    }
}
