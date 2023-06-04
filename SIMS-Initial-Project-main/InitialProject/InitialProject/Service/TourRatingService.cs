using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System.Collections.Generic;
using System.Windows;

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

        public double GetAverageLanguageRating(Tour tour) 
        {
            double counter = 0;
            double sum = 0;

            foreach(TourRating tourRating in GetTourRatings(tour)) 
            {
                if(tourRating.isValid && tourRating.TourId == tourRating.Id)
                {
                    sum += tourRating.GuideLanguage;
                    counter++;
                }
            }
            return sum / counter;
        }

        public TourRating Save(TourRating tourRating)
        {
            return _tourRatingRepository.Save(tourRating);
        }

        public TourRating GetById(int id)
        {
            return _tourRatingRepository.GetById(id);
        }

        public List<TourRating> GetAll()
        {
            return _tourRatingRepository.GetAll();
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
