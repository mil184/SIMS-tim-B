using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.ViewModel.Guest2
{
    public class RateTourViewModel : INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public Guest2TourDTO SelectedTour { get; set; }

        private readonly TourRatingService _tourRatingService;
        private readonly TourReservationService _tourReservationService;
        private readonly TourService _tourService;
        private readonly ImageRepository _imageRepository;

        public int LanguageButtonClickCount { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        private int _guideKnowledge;
        public int GuideKnowledge
        {
            get => _guideKnowledge;
            set
            {
                if (_guideKnowledge != value)
                {
                    _guideKnowledge = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _guideLanguage;
        public int GuideLanguage
        {
            get => _guideLanguage;
            set
            {
                if (_guideLanguage != value)
                {
                    _guideLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _interestingness;
        public int Interestingness
        {
            get => _interestingness;
            set
            {
                if (_interestingness != value)
                {
                    _interestingness = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _imageUrls = new ObservableCollection<string>();
        public ObservableCollection<string> ImageUrls
        {
            get => _imageUrls;
            set
            {
                if (_imageUrls != value)
                {
                    _imageUrls = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<int> _imageIds = new ObservableCollection<int>();

        public RateTourViewModel(Guest2TourDTO selectedTour, Model.User user, TourRatingService tourRatingService, TourReservationService tourReservationService, TourService tourService, ImageRepository imageRepository)
        {
            SelectedTour = selectedTour;
            LoggedInUser = user;

            _tourRatingService = tourRatingService;
            _tourReservationService = tourReservationService;
            _tourService = tourService;
            _imageRepository = imageRepository;

            app = (App)Application.Current;
            app.ChangeLanguage(SRB);
            LanguageButtonClickCount = 0;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Submit()
        {
            var messageBoxResult = MessageBox.Show($"Would you like to save your rating?", "Rating Tour Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.No)
            {
                return;
            }

            TourRating tourRating = new TourRating(
                    SelectedTour.TourId,
                    GuideKnowledge,
                    GuideLanguage,
                    Interestingness,
                    Comment,
                    _imageIds,
                    LoggedInUser.Id);

            Tour tour = _tourService.GetById(tourRating.TourId);
            tour.IsRated = true;
            _tourService.Update(tour);

            TourReservation tourReservation = _tourReservationService.GetReservationByGuestIdAndTourId(LoggedInUser.Id, SelectedTour.TourId);
            tourReservation.IsRated = true;
            _tourReservationService.Update(tourReservation);

            _tourRatingService.Save(tourRating);
        }

        public void AddImage(string urlTextBox)
        {
            string imageUrl = urlTextBox;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                ImageUrls.Add(imageUrl);
                Image image = new Image(imageUrl);
                _imageRepository.Save(image);
                _imageIds.Add(image.Id);
            }
        }

        public void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageButtonClickCount++;

            if (LanguageButtonClickCount % 2 == 1)
            {
                app.ChangeLanguage(ENG);
                return;
            }

            app.ChangeLanguage(SRB);
        }
    }
}
