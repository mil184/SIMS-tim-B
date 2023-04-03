using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Image = InitialProject.Model.Image;

namespace InitialProject.View.Guest1
{
    /// <summary>
    /// Interaction logic for Evaluate.xaml
    /// </summary>
    public partial class Evaluate : Window
    {
        private readonly AccommodationRatingsRepository _accommodationRatingsRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        private readonly AccommodationRatingsDTO SelectedUnratedAccommodation;
        private readonly ImageRepository _imageRepository;

        public AccommodationReservation Reservation { get; set; }

        private int _cleanliness;
        public int Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {
                    _cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _correctness;
        public int Correctness
        {
            get => _correctness;
            set
            {
                if (value != _correctness)
                {
                    _correctness = value;
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

        public Evaluate(AccommodationRatingsDTO selectedUnratedAccommodation, AccommodationRatingsRepository accommodationRatingsRepository, AccommodationReservationRepository accommodationReservationRepository, ImageRepository imageRepository)
        {
            InitializeComponent();
            DataContext = this;

            SelectedUnratedAccommodation = selectedUnratedAccommodation;
            _accommodationRatingsRepository = accommodationRatingsRepository;
            _accommodationReservationRepository = accommodationReservationRepository;
            _imageRepository = imageRepository;

            Reservation = _accommodationReservationRepository.GetById(SelectedUnratedAccommodation.ReservationId);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateComment()) return;
            var messageBoxResult = MessageBox.Show($"Would you like to save your rating?", "Rating Accommodation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
              if (messageBoxResult == MessageBoxResult.Yes)
              {
                    AccommodationRatings accommodationRatings = new AccommodationRatings(Reservation.Id, Reservation.AccommodationId, Reservation.OwnerId, Reservation.GuestId, Cleanliness, Correctness, Comment, _imageIds);
                    _accommodationRatingsRepository.Save(accommodationRatings);
                    MessageBox.Show("Rating saved successfully.");
                    Close();
              }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void AddPicture_Click(object sender, RoutedEventArgs e)
        {
            string imageUrl = UrlTextBox.Text;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                ImageUrls.Add(imageUrl);
                Image image = new Image(imageUrl);
                _imageRepository.Save(image);
                _imageIds.Add(image.Id);
            }
            UrlTextBox.Text = string.Empty;
        }
        private bool ValidateComment()
        {
            if(string.IsNullOrEmpty(commentTextBox.Text))
            {
                ShowNoCommentWarning();
                return false;
            }
            return true;
        }
        private void ShowNoCommentWarning()
        {
            MessageBox.Show("Please enter a comment.", "Comment warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
