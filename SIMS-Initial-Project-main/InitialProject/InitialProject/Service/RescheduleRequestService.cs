using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class RescheduleRequestService
    {
        private readonly IRescheduleRequestRepository _rescheduleRequestRepository;

        public RescheduleRequestService()
        {
            _rescheduleRequestRepository = Injector.CreateInstance<IRescheduleRequestRepository>();
        }

        public List<RescheduleRequest> GetReschedulesByYear(int accommodationId, int year)
        {
            List<RescheduleRequest> reschedulesThisYear = new List<RescheduleRequest>();

            DateTime LeftBoundary = new DateTime(year, 1, 1);
            DateTime RightBoundary = new DateTime(year, 12, 31);

            foreach (RescheduleRequest request in _rescheduleRequestRepository.GetAll())
            {
                if (request.AccommodationId == accommodationId && request.NewStartDate >= LeftBoundary && request.NewStartDate <= RightBoundary)
                {
                    reschedulesThisYear.Add(request);
                }
            }
            return reschedulesThisYear;
        }

        public List<RescheduleRequest> GetReschedulesByMonth(int accommodationId, int year, int month)
        {
            List<RescheduleRequest> reschedulesThisYear = new List<RescheduleRequest>();

            int day;

            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                day = 31;
            }
            else if (month == 2)
            {
                day = 28;
            }
            else
            {
                day = 30;
            }

            DateTime LeftBoundary = new DateTime(year, month, 1);
            DateTime RightBoundary = new DateTime(year, month, day);

            foreach (RescheduleRequest request in _rescheduleRequestRepository.GetAll())
            {
                if (request.AccommodationId == accommodationId && request.NewStartDate >= LeftBoundary && request.NewStartDate <= RightBoundary)
                {
                    reschedulesThisYear.Add(request);
                }
            }
            return reschedulesThisYear;
        }

        public RescheduleRequest GetById(int id)
        {
            return _rescheduleRequestRepository.GetById(id);
        }

        public RescheduleRequest Save(RescheduleRequest rescheduleRequest)
        {
            return _rescheduleRequestRepository.Save(rescheduleRequest);
        }

        public RescheduleRequest Update(RescheduleRequest rescheduleRequest)
        {
            return _rescheduleRequestRepository.Update(rescheduleRequest);
        }

        public List<RescheduleRequest> GetAll()
        {
            return _rescheduleRequestRepository.GetAll();
        }

        public void Subscribe(IObserver observer)
        {
            _rescheduleRequestRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _rescheduleRequestRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _rescheduleRequestRepository.NotifyObservers();
        }
    }
}
