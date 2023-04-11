using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class TourRatingService : ISubject
    {
        private readonly TourRatingRepository _tourRatingRepository;
        private readonly ImageRepository _imageRepository;

        public TourRatingService()
        {
            _tourRatingRepository = new TourRatingRepository();
            _imageRepository = new ImageRepository();
        }

        public List<TourRating> GetTourRatings1(Tour tour)
        {
            List<TourRating> ratings = new List<TourRating>();
            List<TourRating> _tourRatings = _tourRatingRepository.GetAll();

            foreach (TourRating rating in _tourRatings)
            {
                if (rating.TourId == tour.Id)
                {
                    ratings.Add(rating);
                }
            }
            return ratings;
        }

        public List<TourRating> GetTourRatings(Tour tour)
        {
            List<TourRating> tourRatings = _tourRatingRepository.GetAll();
            return tourRatings.FindAll(tourRating => tourRating.TourId == tour.Id);
        }

        public List<string> FindRatingUrls(TourRating rating)
        {
            List<string> urls = new List<string>();

            foreach (int id in rating.ImageIds)
            {
                urls.Add(_imageRepository.GetById(id).Url);
            }

            return urls;
        }

        public TourRating GetById(int id)
        {
            return _tourRatingRepository.GetById(id);
        }

        public TourRating Update(TourRating tourRating)
        {
            return _tourRatingRepository.Update(tourRating);
        }

        public void Subscribe(IObserver observer)
        {
            _tourRatingRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _tourRatingRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _tourRatingRepository.NotifyObservers();
        }
    }
}
