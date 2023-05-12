using InitialProject.Model;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using System.Collections.ObjectModel;

namespace InitialProject.ViewModel.Owner
{
    public class RatingsPageViewModel : IObserver
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<AccommodationRatings> Ratings { get; set; }
        
        public AccommodationRatingService _ratingService;

        public RatingsPageViewModel(User user)
        {
            LoggedInUser = user;

            _ratingService = new AccommodationRatingService();
            _ratingService.Subscribe(this);

            InitializeRatings();
        }

        public void FormRatings()
        {
            foreach (AccommodationRatings rating in _ratingService.GetAvailableRatings(LoggedInUser.Id))
            {
                Ratings.Add(rating);
            }
        }

        public void InitializeRatings()
        {
            Ratings = new ObservableCollection<AccommodationRatings>();
            FormRatings();
        }

        void IObserver.Update()
        {
            Ratings.Clear();
            FormRatings();
        }
    }
}
