using InitialProject.Commands;
using InitialProject.Converters;
using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using iTextSharp.text.pdf.qrcode;
using MenuNavigation.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace InitialProject.ViewModel.Guide
{
    public class ShowCheckpointsViewModel : INotifyPropertyChanged, IObserver
    {
        #region MAIN
        private readonly CheckpointService _checkpointService;
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly UserService _userService;

        public Tour ActiveTour { get; set; }
        public ObservableCollection<UserDTO> UnmarkedGuests { get; set; }
        public UserDTO SelectedGuestDTO { get; set; }
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        private Checkpoint selectedCheckpoint;
        public Checkpoint SelectedCheckpoint
        {
            get { return selectedCheckpoint; }
            set
            {
                if (selectedCheckpoint != value)
                {
                    selectedCheckpoint = value;
                    OnPropertyChanged(nameof(SelectedCheckpoint));
                }
            }
        }
        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value != _selectedIndex)
                {
                    _selectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            UnmarkedGuests.Clear();

            List<int> ids = _tourReservationService.GetAllUserIdsByTour(ActiveTour);
            foreach (int id in ids)
            {
                UnmarkedGuests.Add(GuideDTOConverter.ConvertToDTO(_userService.GetById(id), _tourReservationService, ActiveTour, _checkpointService));
            }
        }

        public ShowCheckpointsViewModel(Tour tour, CheckpointService checkpointService, TourService tourService, TourReservationService tourReservationService, UserService userService) 
        {
             ActiveTour = tour;
            _checkpointService = checkpointService;
            _tourService = tourService;
            _tourReservationService = tourReservationService;
            _userService = userService;

            ActiveTour = tour;

            InitializeCollections();
            SetActiveCheckpoint();
            InitializeOtherValues();
            InitializeCommands();

        }
        private void InitializeOtherValues()
        {
            IsEnabled = true;
            IsDemo = false;
            StopDemo = false;

            ButtonBackgroundColorUpdate = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            ButtonBackgroundColorCheck = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
        }
        private void InitializeCollections() 
        {
            Checkpoints = new ObservableCollection<Checkpoint>(_checkpointService.GetByTour(ActiveTour));
            List<int> ids = _tourReservationService.GetAllUserIdsByTour(ActiveTour);
            UnmarkedGuests = new ObservableCollection<UserDTO>();
            foreach (int id in ids) 
            {
                UnmarkedGuests.Add(GuideDTOConverter.ConvertToDTO(_userService.GetById(id), _tourReservationService, ActiveTour, _checkpointService));
            }
        }
        private void SetActiveCheckpoint()
        {
            SelectedCheckpoint = _checkpointService.GetById(ActiveTour.CurrentCheckpointId);
            
        }

        public void UpdateCheckpoint() 
        {

            SelectedCheckpoint.IsActive = false;
            _checkpointService.Update(SelectedCheckpoint);

            int currentIndex = Checkpoints.IndexOf(SelectedCheckpoint);

            SelectedCheckpoint = Checkpoints[++currentIndex];

            SelectedCheckpoint.IsActive = true;
            _checkpointService.Update(SelectedCheckpoint);

            ActiveTour.CurrentCheckpointId = SelectedCheckpoint.Id;
            _tourService.Update(ActiveTour);

            if (currentIndex == Checkpoints.Count - 1)
            {
                EndTour();
                CancelCommand.Execute(null);
            }

        }

        public void EndTour() 
        {
            ActiveTour.CurrentCheckpointId = -1;
            ActiveTour.IsActive = false;
            ActiveTour.IsFinished = true;
            _tourService.Update(ActiveTour);
            MessageBox.Show($"The {ActiveTour.Name} tour has sucessfuly finished.", "Finished Tour Info", MessageBoxButton.OK, MessageBoxImage.Information);
            CancelCommand.Execute(null);


        }

        public void CheckUser() 
        {
            if(SelectedGuestDTO != null && !_tourReservationService.GetReservationByGuestIdAndTourId(SelectedGuestDTO.UserId, ActiveTour.Id).GuestChecked) 
            {
                TourReservation reservation = _tourReservationService.GetReservationByGuestIdAndTourId(SelectedGuestDTO.UserId, ActiveTour.Id);
                reservation.GuestChecked = true;
                reservation.CheckpointArrivalId = SelectedCheckpoint.Id;
                _tourReservationService.Update(reservation);
                Update();
            }
        }
        #endregion

        #region Demo
        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isDemo;
        public bool IsDemo
        {
            get => _isDemo;
            set
            {
                if (value != _isDemo)
                {
                    _isDemo = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _stopDemo;
        public bool StopDemo
        {
            get => _stopDemo;
            set
            {
                if (value != _stopDemo)
                {
                    _stopDemo = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Duration = 1200;
        private void Empty()
        {
            Checkpoints.Clear();
            UnmarkedGuests.Clear();
        }
        private void Fill() 
        {
            InitializeCollections();
        }
        public async void StartDemoAsync()
        {
            Empty(); 
            IsEnabled = false;
            IsDemo = true;

            while (true)
            {
                if (StopDemo) break;
                await AddGuestsAndCheckpoints();
                if (StopDemo) break;
                await CheckButton();
                if (StopDemo) break;
                await CheckCathy();
                if (StopDemo) break;
                await UpdateButton();
                if (StopDemo) break;
                await CheckpointLouvre();
                if (StopDemo) break;
                await UpdateButton();
                if (StopDemo) break;
                await CheckpointNotre();
                if (StopDemo) break;
                await CheckButton();
                if (StopDemo) break;
                await CheckKaarija();
                if (StopDemo) break;
                await CheckButton();
                if (StopDemo) break;
                await CheckGloria();
                if (StopDemo) break;
                await UpdateButton();
                if (StopDemo) break;
                await CheckpointMontmartre();
                if (StopDemo) break;
                await CheckButton();
                if (StopDemo) break;
                await CheckIris();
                if (StopDemo) break;
                await UpdateButton();
                if (StopDemo) break;
                await CheckpointRiver();
                if (StopDemo) break;
                await ClearAll();
                if (StopDemo) break;
            }
            IsDemo = false;
            StopDemo = false;
            IsEnabled = true;
            Empty();
            Update();

            foreach(Checkpoint c in _checkpointService.GetByTour(ActiveTour)) 
            {
                Checkpoints.Add(c);
            }

            SetActiveCheckpoint();

            if ( ListBox != null & ListBox.SelectedItem == null && ListBox.Items.Count > 0)
            {
                if (SelectedCheckpoint != null)
                {
                    ListBox.SelectedIndex = SelectedCheckpoint.Order - 1;
                }
                else
                {
                    ListBox.SelectedIndex = 0;
                }
            }
        }
        private async Task AddGuestsAndCheckpoints()
        {
            await Task.Delay(Duration);

            UnmarkedGuests.Add(new UserDTO(192, "Gloria", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(194, "Cathy", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(195, "Michael", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(196, "Kaarija", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(197, "Iris", "Not Arrived Yet"));

            Checkpoints.Add(new Checkpoint("Eiffel Tower", 1));
            SelectedCheckpoint = Checkpoints[0];
            Checkpoints.Add(new Checkpoint("Louvre Museum", 2));
            Checkpoints.Add(new Checkpoint("Notre-Dame Cathedral", 3));
            Checkpoints.Add(new Checkpoint("Montmartre", 4));
            Checkpoints.Add(new Checkpoint("Seine River Cruise", 5));

            await Task.Delay(Duration);
        }
        private async Task CheckCathy()
        {
            UnmarkedGuests.Clear();
            UnmarkedGuests.Add(new UserDTO(192, "Gloria", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(194, "Cathy", "Eiffel Tower"));
            UnmarkedGuests.Add(new UserDTO(195, "Michael", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(196, "Kaarija", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(197, "Iris", "Not Arrived Yet"));


            SelectedGuestDTO = UnmarkedGuests[1];

            await Task.Delay(Duration);
        }
        private async Task CheckpointLouvre()
        {
            SelectedCheckpoint = Checkpoints[1];
            await Task.Delay(Duration);
        }
        private async Task CheckpointNotre()
        {
            SelectedCheckpoint = Checkpoints[2];
            await Task.Delay(Duration);
        }
        private async Task CheckKaarija()
        {
            UnmarkedGuests.Clear();
            UnmarkedGuests.Add(new UserDTO(192, "Gloria", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(194, "Cathy", "Eiffel Tower"));
            UnmarkedGuests.Add(new UserDTO(195, "Michael", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(196, "Kaarija", "Notre-Dame Cathedral"));
            UnmarkedGuests.Add(new UserDTO(197, "Iris", "Not Arrived Yet"));

            await Task.Delay(Duration);
        }
        private async Task CheckGloria()
        {
            UnmarkedGuests.Clear();
            UnmarkedGuests.Add(new UserDTO(192, "Gloria", "Notre-Dame Cathedral"));
            UnmarkedGuests.Add(new UserDTO(194, "Cathy", "Eiffel Tower"));
            UnmarkedGuests.Add(new UserDTO(195, "Michael", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(196, "Kaarija", "Notre-Dame Cathedral"));
            UnmarkedGuests.Add(new UserDTO(197, "Iris", "Not Arrived Yet"));

            await Task.Delay(Duration);
        }
        private async Task CheckpointMontmartre()
        {
            SelectedCheckpoint = Checkpoints[3];
            await Task.Delay(Duration);
        }
        private async Task CheckIris()
        {
            UnmarkedGuests.Clear();
            UnmarkedGuests.Add(new UserDTO(192, "Gloria", "Notre-Dame Cathedral"));
            UnmarkedGuests.Add(new UserDTO(194, "Cathy", "Eiffel Tower"));
            UnmarkedGuests.Add(new UserDTO(195, "Michael", "Not Arrived Yet"));
            UnmarkedGuests.Add(new UserDTO(196, "Kaarija", "Notre-Dame Cathedral"));
            UnmarkedGuests.Add(new UserDTO(197, "Iris", "Montmartre"));

            await Task.Delay(Duration);
        }
        private async Task CheckpointRiver()
        {
            SelectedCheckpoint = Checkpoints[4];
            await Task.Delay(Duration);
        }
        private async Task ClearAll()
        {
            Empty();
        }
        private async Task CheckButton()
        {
            ButtonBackgroundColorCheck = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00415E"));
            await Task.Delay(200);
            ButtonBackgroundColorCheck = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
        }
        private async Task UpdateButton()
        {
            ButtonBackgroundColorUpdate = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00415E"));
            await Task.Delay(200);
            ButtonBackgroundColorUpdate = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
        }
        private SolidColorBrush _buttonBackgroundColorUpdate;
        public SolidColorBrush ButtonBackgroundColorUpdate
        {
            get { return _buttonBackgroundColorUpdate; }
            set
            {
                _buttonBackgroundColorUpdate = value;
                OnPropertyChanged(nameof(ButtonBackgroundColorUpdate));
            }
        }
        private SolidColorBrush _buttonBackgroundColorCheck;
        public SolidColorBrush ButtonBackgroundColorCheck
        {
            get { return _buttonBackgroundColorCheck; }
            set
            {
                _buttonBackgroundColorCheck = value;
                OnPropertyChanged(nameof(ButtonBackgroundColorCheck));
            }
        }
        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand EndTourCommand { get; set; }
        public RelayCommand UpdateCheckpointCommand { get; private set; }
        public RelayCommand DemoCommand { get; private set; }
        public ICommand StopDemoCommand { get; private set; }
        public RelayCommand EnterCommand { get; private set; }
        public ICommand ListBoxLoadedCommand { get; set; }
        public ListBox ListBox { get; set; }
        public void InitializeCommands()
        {
            UpdateCheckpointCommand = new RelayCommand(UpdateCheckpoint, CanExecute);
            DemoCommand = new RelayCommand(StartDemo, CanExecute);
            EnterCommand = new RelayCommand(Enter, CanExecute);
            EndTourCommand= new RelayCommand(EndTour, CanExecute);
            StopDemoCommand = new DelegateCommand(StopDemoM, CannotExecute);
            ListBoxLoadedCommand = new RelayCommand(ExecuteListBoxLoadedCommand);
        }
        private void ExecuteListBoxLoadedCommand(object parameter)
        {
            if (parameter is ListBox listBox)
            {
                if (listBox.SelectedItem == null && listBox.Items.Count > 0)
                {
                    ListBox = listBox;
                    if (SelectedCheckpoint != null)
                    {
                        listBox.SelectedIndex = SelectedCheckpoint.Order - 1;
                    }
                    else
                    {
                        listBox.SelectedIndex = 0;
                    }
                }
            }
        }
        private void StopDemoM(object parameter)
        {
            StopDemo = true;
        }
        private bool CanExecute(object parameter)
        {
            return !IsDemo;
        }
        private bool CannotExecute(object parameter)
        {
            return IsDemo;
        }
        private void StartDemo(object parameter)
        {
            StartDemoAsync();
        }
        private void UpdateCheckpoint(object parameter)
        {
            UpdateCheckpoint();
        }
        private void EndTour(object parameter)
        {
            EndTour();
        }
        private void Enter(object parameter)
        {
         CheckUser();
        }
        #endregion
    }
}
