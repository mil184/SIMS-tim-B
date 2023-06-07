using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using MenuNavigation.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.ViewModel.Guest2
{
    public class RateTourViewModel : INotifyPropertyChanged
    {
        public Action CloseAction { get; set; }
        public User LoggedInUser { get; set; }
        public Guest2TourDTO SelectedTour { get; set; }

        private readonly TourRatingService _tourRatingService;
        private readonly TourReservationService _tourReservationService;
        private readonly TourService _tourService;
        private readonly ImageRepository _imageRepository;

        public RelayCommand SubmitRatingCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand AddImageCommand { get; set; }
        public RelayCommand ToursImage_MouseEnter { get; set; }
        public RelayCommand ToursImage_MouseLeave { get; set; }

        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        #region Properties

        public bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen != value)
                {
                    _isOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private bool _knowledge1IsChecked;
        public bool Knowledge1IsChecked
        {
            get => _knowledge1IsChecked;
            set
            {
                if (value != _knowledge1IsChecked)
                {
                    _knowledge1IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _knowledge2IsChecked;
        public bool Knowledge2IsChecked
        {
            get => _knowledge2IsChecked;
            set
            {
                if (value != _knowledge2IsChecked)
                {
                    _knowledge2IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _knowledge3IsChecked;
        public bool Knowledge3IsChecked
        {
            get => _knowledge3IsChecked;
            set
            {
                if (value != _knowledge3IsChecked)
                {
                    _knowledge3IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _knowledge4IsChecked;
        public bool Knowledge4IsChecked
        {
            get => _knowledge4IsChecked;
            set
            {
                if (value != _knowledge4IsChecked)
                {
                    _knowledge4IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _knowledge5IsChecked;
        public bool Knowledge5IsChecked
        {
            get => _knowledge5IsChecked;
            set
            {
                if (value != _knowledge5IsChecked)
                {
                    _knowledge5IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _interesting1IsChecked;
        public bool Interesting1IsChecked
        {
            get => _interesting1IsChecked;
            set
            {
                if (value != _interesting1IsChecked)
                {
                    _interesting1IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _interesting2IsChecked;
        public bool Interesting2IsChecked
        {
            get => _interesting2IsChecked;
            set
            {
                if (value != _interesting2IsChecked)
                {
                    _interesting2IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _interesting3IsChecked;
        public bool Interesting3IsChecked
        {
            get => _interesting3IsChecked;
            set
            {
                if (value != _interesting3IsChecked)
                {
                    _interesting3IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _interesting4IsChecked;
        public bool Interesting4IsChecked
        {
            get => _interesting4IsChecked;
            set
            {
                if (value != _interesting4IsChecked)
                {
                    _interesting4IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _interesting5IsChecked;
        public bool Interesting5IsChecked
        {
            get => _interesting5IsChecked;
            set
            {
                if (value != _interesting5IsChecked)
                {
                    _interesting5IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _language1IsChecked;
        public bool Language1IsChecked
        {
            get => _language1IsChecked;
            set
            {
                if (value != _language1IsChecked)
                {
                    _language1IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _language2IsChecked;
        public bool Language2IsChecked
        {
            get => _language2IsChecked;
            set
            {
                if (value != _language2IsChecked)
                {
                    _language2IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _language3IsChecked;
        public bool Language3IsChecked
        {
            get => _language3IsChecked;
            set
            {
                if (value != _language3IsChecked)
                {
                    _language3IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _language4IsChecked;
        public bool Language4IsChecked
        {
            get => _language4IsChecked;
            set
            {
                if (value != _language4IsChecked)
                {
                    _language4IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _language5IsChecked;
        public bool Language5IsChecked
        {
            get => _language5IsChecked;
            set
            {
                if (value != _language5IsChecked)
                {
                    _language5IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        public RateTourViewModel(Guest2TourDTO selectedTour, User user, TourRatingService tourRatingService, TourReservationService tourReservationService, TourService tourService, ImageRepository imageRepository, string lang)
        {
            SelectedTour = selectedTour;
            LoggedInUser = user;

            _tourRatingService = tourRatingService;
            _tourReservationService = tourReservationService;
            _tourService = tourService;
            _imageRepository = imageRepository;

            SubmitRatingCommand = new RelayCommand(Execute_SubmitRatingCommand, CanExecute_SubmitRatingCommand);
            ExitCommand = new RelayCommand(Execute_ExitCommand);
            AddImageCommand = new RelayCommand(Execute_AddImageCommand);
            ToursImage_MouseEnter = new RelayCommand(Execute_ToursImage_MouseEnter);
            ToursImage_MouseLeave = new RelayCommand(Execute_ToursImage_MouseLeave);

            app = (App)Application.Current;
            app.ChangeLanguage(lang);
        }

        private void Execute_ToursImage_MouseLeave(object obj)
        {
            IsOpen = false;
        }

        private void Execute_ToursImage_MouseEnter(object obj)
        {
            IsOpen = true;
        }

        private void Execute_AddImageCommand(object obj)
        {
            AddImage(ImageUrl);
            ImageUrl = string.Empty;
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

        private void Execute_ExitCommand(object obj)
        {
            CloseAction();
        }

        private bool CanExecute_SubmitRatingCommand(object obj)
        {
            SetRatingsForNumberProperties();

            return GuideKnowledge != 0 &&
                Interestingness != 0 &&
                GuideLanguage != 0 &&
                Comment != null &&
                ImageUrls.Count != 0;
        }

        private void Execute_SubmitRatingCommand(object obj)
        {
            SetRatingsForNumberProperties();

            if (GuideKnowledge == 0)
            {
                MessageBox.Show("Please rate your guide's knowledge!", "Guide knowledge warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                // return;
            }

            else if (Interestingness == 0)
            {
                MessageBox.Show("Please rate tour interestingness!", "Interestingness warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                // return;
            }

            else if (GuideLanguage == 0)
            {
                MessageBox.Show("Please rate your guide's language!", "Guide language warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                //return;
            }

            else if (Comment == null)
            {
                MessageBox.Show("Please add a comment!", "Comment warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                //return;
            }

            else if (ImageUrls.Count == 0)
            {
                MessageBox.Show("Please add at least one image!", "Images warning", MessageBoxButton.OK, MessageBoxImage.Warning);
               //return;
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

            MessageBox.Show("Tour successfuly rated!");
            CloseAction();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region NumberPropertySetters

        private void SetRatingsForNumberProperties()
        {
            SetRatingsForGuideKnowledge();
            SetRatingsForInterestingness();
            SetRatingsForGuideLanguage();
        }

        private void SetRatingsForGuideKnowledge()
        {
            if (Knowledge1IsChecked)
            {
                GuideKnowledge = 1;
            }
            else if (Knowledge2IsChecked)
            {
                GuideKnowledge = 2;
            }
            else if (Knowledge3IsChecked)
            {
                GuideKnowledge = 3;
            }
            else if (Knowledge4IsChecked)
            {
                GuideKnowledge = 4;
            }
            else if (Knowledge5IsChecked)
            {
                GuideKnowledge = 5;
            }
        }

        private void SetRatingsForInterestingness()
        {
            if (Interesting1IsChecked)
            {
                Interestingness = 1;
            }
            else if (Interesting2IsChecked)
            {
                Interestingness = 2;
            }
            else if (Interesting3IsChecked)
            {
                Interestingness = 3;
            }
            else if (Interesting4IsChecked)
            {
                Interestingness = 4;
            }
            else if (Interesting5IsChecked)
            {
                Interestingness = 5;
            }
        }

        private void SetRatingsForGuideLanguage()
        {
            if (Language1IsChecked)
            {
                GuideLanguage = 1;
            }
            else if (Language2IsChecked)
            {
                GuideLanguage = 2;
            }
            else if (Language3IsChecked)
            {
                GuideLanguage = 3;
            }
            else if (Language4IsChecked)
            {
                GuideLanguage = 4;
            }
            else if (Language5IsChecked)
            {
                GuideLanguage = 5;
            }
        }

        #endregion
    }
}
