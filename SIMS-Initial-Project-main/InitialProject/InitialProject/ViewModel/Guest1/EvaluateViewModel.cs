using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.View.Guest1;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MenuNavigation.Commands;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Injector;

namespace InitialProject.ViewModel.Guest1
{
    public class EvaluateViewModel: IDataErrorInfo
    {
        private readonly AccommodationRatingService _accommodationRatingsService;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userService;
        private readonly RenovationRecommendationService _renovationRecommendationService;

        public AccommodationReservation Reservation { get; set; }

        public RelayCommand SaveEvaluateCommand { get; set; }
        public RelayCommand CancelEvaluateCommand { get; set; }
        public RelayCommand AddPictureCommand { get; set; }
        public RelayCommand RecommendRenovationCommand { get; set; }

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

        private string _url;
        public string Url
        {
            get => _url;
            set
            {
                if (value != _url)
                {
                    _url = value;
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

        private bool _cleanliness1IsChecked;
        public bool Cleanliness1IsChecked
        {
            get => _cleanliness1IsChecked;
            set
            {
                if (value != _cleanliness1IsChecked)
                {
                    _cleanliness1IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _cleanliness2IsChecked;
        public bool Cleanliness2IsChecked
        {
            get => _cleanliness2IsChecked;
            set
            {
                if (value != _cleanliness2IsChecked)
                {
                    _cleanliness2IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _cleanliness3IsChecked;
        public bool Cleanliness3IsChecked
        {
            get => _cleanliness3IsChecked;
            set
            {
                if (value != _cleanliness3IsChecked)
                {
                    _cleanliness3IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _cleanliness4IsChecked;
        public bool Cleanliness4IsChecked
        {
            get => _cleanliness4IsChecked;
            set
            {
                if (value != _cleanliness4IsChecked)
                {
                    _cleanliness4IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _cleanliness5IsChecked;
        public bool Cleanliness5IsChecked
        {
            get => _cleanliness5IsChecked;
            set
            {
                if (value != _cleanliness5IsChecked)
                {
                    _cleanliness5IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness1IsChecked;
        public bool Correctness1IsChecked
        {
            get => _correctness1IsChecked;
            set
            {
                if (value != _correctness1IsChecked)
                {
                    _correctness1IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness2IsChecked;
        public bool Correctness2IsChecked
        {
            get => _correctness2IsChecked;
            set
            {
                if (value != _correctness2IsChecked)
                {
                    _correctness2IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness3IsChecked;
        public bool Correctness3IsChecked
        {
            get => _correctness3IsChecked;
            set
            {
                if (value != _correctness3IsChecked)
                {
                    _correctness3IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness4IsChecked;
        public bool Correctness4IsChecked
        {
            get => _correctness4IsChecked;
            set
            {
                if (value != _correctness4IsChecked)
                {
                    _correctness4IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _correctness5IsChecked;
        public bool Correctness5IsChecked
        {
            get => _correctness5IsChecked;
            set
            {
                if (value != _correctness5IsChecked)
                {
                    _correctness5IsChecked = value;
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
                    OnPropertyChanged(nameof(OwnerName));
                }
            }
        }
        public string AccommodationName
        {
            get => SelectedUnratedAccommodation?.AccommodationName;
        }
        public string OwnerName
        {
            get => SelectedUnratedAccommodation?.UserName;
        }

        public EvaluateViewModel(AccommodationRatingsDTO selectedUnratedAccommodation, AccommodationRatingService accommodationRatingsService, AccommodationReservationService accommodationReservationService, IImageRepository imageRepository)
        {
            SelectedUnratedAccommodation = selectedUnratedAccommodation;
            _accommodationRatingsService = accommodationRatingsService;
            _accommodationReservationService = accommodationReservationService;
            _imageRepository = imageRepository;
            _userService = Injector.CreateInstance<IUserRepository>();
            _renovationRecommendationService = new RenovationRecommendationService();

            Reservation = _accommodationReservationService.GetById(SelectedUnratedAccommodation.ReservationId);

            SaveEvaluateCommand = new RelayCommand(Execute_SaveEvaluateCommand, CanExecute_Command);
            CancelEvaluateCommand = new RelayCommand(Execute_CancelEvaluateCommand, CanExecute_Command);
            AddPictureCommand = new RelayCommand(Execute_AddPictureCommand, CanExecute_Command);
            RecommendRenovationCommand = new RelayCommand(Execute_RecommendRenovationCommand, CanExecute_Command);
        }

        private void SetRatingsForCleanlinees()
        {
            if (Cleanliness1IsChecked == true)
            {
                Cleanliness = 1;
            }
            else if (Cleanliness2IsChecked == true)
            {
                Cleanliness = 2;
            }
            else if (Cleanliness3IsChecked == true)
            {
                Cleanliness = 3;
            }
            else if (Cleanliness4IsChecked == true)
            {
                Cleanliness = 4;
            }
            else if (Cleanliness5IsChecked == true)
            {
                Cleanliness = 5;
            }
        }

        private void SetRatingsForCorrectness()
        {
            if (Correctness1IsChecked == true)
            {
                Correctness = 1;
            }
            else if (Correctness2IsChecked == true)
            {
                Correctness = 2;
            }
            else if (Correctness3IsChecked == true)
            {
                Correctness = 3;
            }
            else if (Correctness4IsChecked == true)
            {
                Correctness = 4;
            }
            else if (Correctness5IsChecked == true)
            {
                Correctness = 5;
            }
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        private void Execute_SaveEvaluateCommand(object obj)
        {
            if (IsValid)
            {
                var messageBoxResult = MessageBox.Show($"Would you like to save your rating?", "Rating Accommodation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    SetRatingsForCleanlinees();
                    SetRatingsForCorrectness();
                    AccommodationRatings accommodationRatings = new AccommodationRatings(Reservation.Id, Reservation.AccommodationId, Reservation.OwnerId, Reservation.GuestId, Cleanliness, Correctness, Comment, _imageIds);
                    AccommodationReservation reservation = _accommodationReservationService.GetById(Reservation.Id);
                    reservation.IsRated = true;
                    _accommodationReservationService.Update(reservation);
                    _accommodationRatingsService.Save(accommodationRatings);
                    MessageBox.Show("Rating saved successfully.");
                    CheckForSuperOwnerPrivileges(Reservation.OwnerId);
                    var window = Application.Current.Windows.OfType<Evaluate>().FirstOrDefault();
                    window.Close();
                }
            }
        }

        private void CheckForSuperOwnerPrivileges(int id)
        {
            int numberOfRatings = _accommodationRatingsService.GetAll().Where(x => x.OwnerId == id).Count();
            int cleanlinessSum = _accommodationRatingsService.GetAll().Where(x => x.OwnerId == id).Sum(x => x.Cleanliness);
            int correctnessSum = _accommodationRatingsService.GetAll().Where(x => x.OwnerId == id).Sum(x => x.Correctness);

            double averageRating = (cleanlinessSum + correctnessSum) / (numberOfRatings * 2);

            SetSuperOwnerPrivileges(id, numberOfRatings, averageRating);
        }

        private void SetSuperOwnerPrivileges(int id, int numberOfRatings, double averageRating)
        {
            User owner = _userService.GetById(id);
            if (numberOfRatings >= 50 && averageRating > 4.5)
            {
                owner.Type = InitialProject.Resources.Enums.UserType.superowner;
            }
            else
            {
                owner.Type = InitialProject.Resources.Enums.UserType.owner;
            }
            _userService.Update(owner);
        }

        private void Execute_CancelEvaluateCommand(object obj)
        {
            var window = Application.Current.Windows.OfType<Evaluate>().FirstOrDefault();
            window.Close();
        }


        private void Execute_AddPictureCommand(object obj)
        {
            if (!string.IsNullOrEmpty(Url))
            {
                ImageUrls.Add(Url);
                Image image = new Image(Url);
                _imageRepository.Save(image);
                _imageIds.Add(image.Id);
            }
            Url = string.Empty;
        }

        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                string error = null;

                if (columnName == "Comment" && string.IsNullOrEmpty(Comment))
                {
                    error = "Comment is required!";
                }
                if(columnName == "Cleanliness" && !Cleanliness1IsChecked && !Cleanliness2IsChecked && !Cleanliness3IsChecked && !Cleanliness4IsChecked && !Cleanliness5IsChecked)
                {
                    error = "Must be selected one option for cleanliness!";
                }
                if (columnName == "Correctness" && !Correctness1IsChecked && !Correctness2IsChecked && !Correctness3IsChecked && !Correctness4IsChecked && !Correctness5IsChecked)
                {
                    error = "Must be selected one option for correctness!";
                }
                return error;
            }

        }

        private readonly string[] _validatedProperties = { "Comment", "Cleanliness", "Correctness" };

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

        private void Execute_RecommendRenovationCommand(object obj)
        {
            RecommendRenovation recommendRenovation = new RecommendRenovation(SelectedUnratedAccommodation, _renovationRecommendationService, _accommodationReservationService);
            recommendRenovation.ShowDialog();
        }
    }
}
