using InitialProject.Model;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System.Collections.Generic;
using System.Linq;
using Xceed.Wpf.Toolkit;

namespace InitialProject.Service
{
    public class AccommodationRatingService
    {
        private readonly IAccommodationRatingRepository _accommodationRatingRepository;
        private readonly GuestReviewService _guestReviewService;

        public AccommodationRatingService()
        {
            _accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();
            _guestReviewService = new GuestReviewService();
        }

        public double GetAverageRatings(string gradeType, int ownerId)
        {
            double grade = 0;
            int counter = 0;

            foreach (var rating in _accommodationRatingRepository.GetAll())
            {
                if (rating.OwnerId == ownerId)
                {
                    if (gradeType == "Cleanliness")
                    {
                        grade += rating.Cleanliness;
                    }
                    else
                    {
                        grade += rating.Correctness;
                    }
                    counter++;
                }
            }

            if (counter == 0)
            {
                return 0;
            }

            return grade / counter;
        }

        public double GetAverageRatings(int accommodationId, string gradeType)
        {
            double grade = 0;
            int counter = 0;

            foreach (var rating in _accommodationRatingRepository.GetAll())
            {
                if (rating.AccommodationId == accommodationId)
                {
                    if (gradeType == "Cleanliness")
                    {
                        grade += rating.Cleanliness;
                    }
                    else
                    {
                        grade += rating.Correctness;
                    }
                    counter++;
                }
            }

            if (counter == 0)
            {
                return 0;
            }

            return grade / counter;
        }

        public List<AccommodationRatings> GetAvailableRatings(int ownerId)
        {
            var ratings = new List<AccommodationRatings>();

            foreach (var rating in _accommodationRatingRepository.GetAll())
            {
                if (rating.OwnerId == ownerId && _guestReviewService.GetAll().Any(t => t.ReservationId == rating.ReservationId))
                {
                    ratings.Add(rating);
                }
            }

            return ratings;
        }

        public AccommodationRatings Save(AccommodationRatings accommodationRatings)
        {
            return _accommodationRatingRepository.Save(accommodationRatings);
        }

        public AccommodationRatings Update(AccommodationRatings accommodationRatings)
        {
            return _accommodationRatingRepository.Update(accommodationRatings);
        }

        public List<AccommodationRatings> GetAll()
        {
            return _accommodationRatingRepository.GetAll();
        }

        public void Subscribe(IObserver observer)
        {
            _accommodationRatingRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _accommodationRatingRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _accommodationRatingRepository.NotifyObservers();
        }
    }
}
