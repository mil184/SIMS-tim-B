using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using InitialProject.View.Guest1;
using MenuNavigation.Commands;

namespace InitialProject.ViewModel.Guest1
{
    public class RecommendRenovationViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly RenovationRecommendationService _renovationRecommendationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        public AccommodationReservation Reservation { get; set; }

        public RelayCommand SendRecommendationCommand { get; set; }
        public RelayCommand CancelRecommendationCommand { get; set; }

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

        private bool _checkbox1IsChecked;
        public bool Checkbox1IsChecked
        {
            get => _checkbox1IsChecked;
            set 
            {
                if (value != _checkbox1IsChecked)
                {
                    _checkbox1IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _checkbox2IsChecked;
        public bool Checkbox2IsChecked
        {
            get => _checkbox2IsChecked;
            set
            {
                if (value != _checkbox2IsChecked)
                {
                    _checkbox2IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _checkbox3IsChecked;
        public bool Checkbox3IsChecked
        {
            get => _checkbox3IsChecked;
            set
            {
                if (value != _checkbox3IsChecked)
                {
                    _checkbox3IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _checkbox4IsChecked;
        public bool Checkbox4IsChecked
        {
            get => _checkbox4IsChecked;
            set
            {
                if (value != _checkbox4IsChecked)
                {
                    _checkbox4IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _checkbox5IsChecked;
        public bool Checkbox5IsChecked
        {
            get => _checkbox5IsChecked;
            set
            {
                if (value != _checkbox5IsChecked)
                {
                    _checkbox5IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private AccommodationRatingsDTO _selectedUnratedAccommodation;
        public AccommodationRatingsDTO SelectedUnratedAccommodation
        {
            get => _selectedUnratedAccommodation;
            set
            {
                if (value != _selectedUnratedAccommodation)
                {
                    _selectedUnratedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedUnratedAccommodation));
                    OnPropertyChanged(nameof(AccommodationName));
                }
            }
        }
        public string AccommodationName
        {
            get => SelectedUnratedAccommodation?.AccommodationName;
        }
        public RecommendRenovationViewModel(AccommodationRatingsDTO selectedUnratedAccommodation, RenovationRecommendationService renovationRecommendationService, AccommodationReservationService accommodationReservationService)
        {
            SelectedUnratedAccommodation = selectedUnratedAccommodation;
            _renovationRecommendationService = renovationRecommendationService;
            _accommodationReservationService = accommodationReservationService;

            Reservation = _accommodationReservationService.GetById(SelectedUnratedAccommodation.ReservationId);

            SendRecommendationCommand = new RelayCommand(Execute_SendRecommendationCommand, CanExecute_Command);
            CancelRecommendationCommand = new RelayCommand(Execute_CancelRecommendationCommand, CanExecute_Command);
        }

        private void SetValuesForRenovationLevel()
        {
            if (Checkbox1IsChecked == true)
            {
                RenovationLevel = 1;
            }
            else if (Checkbox2IsChecked == true)
            {
                RenovationLevel = 2;
            }
            else if (Checkbox3IsChecked == true)
            {
                RenovationLevel = 3;
            }
            else if (Checkbox4IsChecked == true)
            {
                RenovationLevel = 4;
            }
            else if (Checkbox5IsChecked == true)
            {
                RenovationLevel = 5;
            }
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        private void Execute_CancelRecommendationCommand(object obj)
        {
            var window = Application.Current.Windows.OfType<RecommendRenovation>().FirstOrDefault();
            window.Close();
        }

        private void Execute_SendRecommendationCommand(object obj)
        {
            if (IsValid)
            {
                var messageBoxResult = MessageBox.Show($"Would you like to send your recommendation?", "Send Recommendation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    SetValuesForRenovationLevel();
                    RenovationRecommendation renovationRecommendation = new RenovationRecommendation(Reservation.AccommodationId, Information, RenovationLevel, Reservation.GuestId, DateTime.Now);
                    _renovationRecommendationService.Save(renovationRecommendation);
                    MessageBox.Show("Recommendation for renovation sent successfully.");
                    var window = Application.Current.Windows.OfType<RecommendRenovation>().FirstOrDefault();
                    window.Close();
                }
            }
        }

        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                string error = null;

                if (columnName == "Information" && string.IsNullOrEmpty(Information))
                {
                    error = "Information about the state is required!";
                }
                if (columnName == "RenovationLevel" && !Checkbox1IsChecked && !Checkbox2IsChecked && !Checkbox3IsChecked && !Checkbox4IsChecked && !Checkbox5IsChecked)
                {
                    error = "Must be selected one option for level of renovation urgency!";
                }
                return error;
            }

        }

        private readonly string[] _validatedProperties = { "Information", "RenovationLevel" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
