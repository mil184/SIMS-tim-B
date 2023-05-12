using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
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

namespace InitialProject.View.Guest1
{
    /// <summary>
    /// Interaction logic for RecommendRenovation.xaml
    /// </summary>
    public partial class RecommendRenovation : Window
    {
        private readonly AccommodationRatingsRepository _accommodationRatingsRepository;
        private readonly RenovationRecommendationRepository _renovationRecommendationRepository;
        private readonly AccommodationRatingsDTO SelectedUnratedAccommodation;
        private readonly AccommodationReservationService _accommodationReservationService;
        public AccommodationReservation Reservation { get; set; }


        private string _information;
        public string Information
        {
            get => _information;
            set
            {
                if (value != _information)
                {
                    _information = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _renovationLevel;
        public int RenovationLevel
        {
            get => _renovationLevel;
            set
            {
                if (value != _renovationLevel)
                {
                    _renovationLevel = value;
                    OnPropertyChanged();
                }
            }
        }
        public RecommendRenovation(AccommodationRatingsDTO selectedUnratedAccommodation, RenovationRecommendationRepository renovationRecommendationRepository, AccommodationReservationService accommodationReservationService)
        {
            InitializeComponent();
            DataContext = this;

            SelectedUnratedAccommodation = selectedUnratedAccommodation;
            _renovationRecommendationRepository = renovationRecommendationRepository;
            _accommodationReservationService = accommodationReservationService;
            _accommodationRatingsRepository = new AccommodationRatingsRepository();

            Reservation = _accommodationReservationService.GetById(SelectedUnratedAccommodation.ReservationId);
        }

        private void SetValuesForRenovationLevel()
        {
            if (checkbox1.IsChecked == true)
            {
                RenovationLevel = 1;
            }
            else if (checkbox2.IsChecked == true)
            {
                RenovationLevel = 2;
            }
            else if (checkbox3.IsChecked == true)
            {
                RenovationLevel = 3;
            }
            else if (checkbox4.IsChecked == true)
            {
                RenovationLevel = 4;
            }
            else if (checkbox5.IsChecked == true)
            {
                RenovationLevel = 5;
            }
        }

        private void CancelRecommendation_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SendRecommendation_Click(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = MessageBox.Show($"Would you like to send your recommendation?", "Send Recommendation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SetValuesForRenovationLevel();
                RenovationRecommendation renovationRecommendation = new RenovationRecommendation(Reservation.AccommodationId, Information, RenovationLevel, Reservation.GuestId);
                _renovationRecommendationRepository.Save(renovationRecommendation);
                MessageBox.Show("Recommendation for renovation sent successfully.");
                Close();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
