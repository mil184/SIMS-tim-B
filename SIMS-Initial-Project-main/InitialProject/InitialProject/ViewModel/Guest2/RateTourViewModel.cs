using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.ViewModel.Guest2
{
    public class RateTourViewModel : INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public Guest2TourDTO SelectedTour { get; set; }

        private readonly TourRatingRepository _tourRatingRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourService _tourService;
        private readonly ImageRepository _imageRepository;

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

        public RateTourViewModel(Guest2TourDTO selectedTour, Model.User user, TourRatingRepository tourRatingRepository, TourReservationRepository tourReservationRepository, TourService tourService, ImageRepository imageRepository)
        {
            SelectedTour = selectedTour;
            LoggedInUser = user;

            _tourRatingRepository = tourRatingRepository;
            _tourReservationRepository = tourReservationRepository;
            _tourService = tourService;
            _imageRepository = imageRepository;
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

            TourReservation tourReservation = _tourReservationRepository.GetReservationByGuestIdAndTourId(LoggedInUser.Id, SelectedTour.TourId);
            tourReservation.IsRated = true;
            _tourReservationRepository.Update(tourReservation);

            _tourRatingRepository.Save(tourRating);
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
    }
}
