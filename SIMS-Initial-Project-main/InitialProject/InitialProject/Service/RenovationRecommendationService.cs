using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class RenovationRecommendationService
    {
        private readonly IRenovationRecommendationRepository _renovationRecommendationRepository;

        public RenovationRecommendationService()
        {
            _renovationRecommendationRepository= Injector.CreateInstance<IRenovationRecommendationRepository>();
        }

        public List<RenovationRecommendation> GetRecommendationsByYear(int accommodationId, int year)
        {
            List<RenovationRecommendation> recommendationsThisYear = new List<RenovationRecommendation>();

            DateTime LeftBoundary = new DateTime(year, 1, 1);
            DateTime RightBoundary = new DateTime(year, 12, 31);

            foreach (RenovationRecommendation recommendation in _renovationRecommendationRepository.GetAll())
            {
                if (recommendation.AccommodationId == accommodationId && recommendation.RecommendationDate >= LeftBoundary && recommendation.RecommendationDate <= RightBoundary)
                {
                    recommendationsThisYear.Add(recommendation);
                }
            }
            return recommendationsThisYear;
        }

        public List<RenovationRecommendation> GetRecommendationsByMonth(int accommodationId, int year, int month)
        {
            List<RenovationRecommendation> recommendationsThisYear = new List<RenovationRecommendation>();

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

            foreach (RenovationRecommendation recommendation in _renovationRecommendationRepository.GetAll())
            {
                if (recommendation.AccommodationId == accommodationId && recommendation.RecommendationDate >= LeftBoundary && recommendation.RecommendationDate <= RightBoundary)
                {
                    recommendationsThisYear.Add(recommendation);
                }
            }
            return recommendationsThisYear;
        }

        public RenovationRecommendation Save(RenovationRecommendation renovationRecommendation)
        {
            return _renovationRecommendationRepository.Save(renovationRecommendation);
        }

        public RenovationRecommendation Update(RenovationRecommendation renovationRecommendation)
        {
            return _renovationRecommendationRepository.Update(renovationRecommendation);
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _renovationRecommendationRepository.GetAll();
        }

        public void Subscribe(IObserver observer)
        {
            _renovationRecommendationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _renovationRecommendationRepository.Subscribe(observer);
        }

        public void NotifyObservers()
        {
            _renovationRecommendationRepository.NotifyObservers();
        }
    }
}
