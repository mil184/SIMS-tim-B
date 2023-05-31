using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Service
{
    public class AccommodationRatingService
    {
        private readonly IAccommodationRatingRepository _accommodationRatingRepository;
        private readonly IGuestReviewRepository _guestReviewRepository;

        public AccommodationRatingService()
        {
            _accommodationRatingRepository = Injector.CreateInstance<IAccommodationRatingRepository>();
            _guestReviewRepository = Injector.CreateInstance <IGuestReviewRepository>();
        }

        public List<AccommodationRatings> GetAvailableRatings(int ownerId)
        {
            var ratings = new List<AccommodationRatings>();

            foreach (var rating in _accommodationRatingRepository.GetAll())
            {
                if (rating.OwnerId == ownerId && _guestReviewRepository.GetAll().Any(t => t.ReservationId == rating.ReservationId))
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
