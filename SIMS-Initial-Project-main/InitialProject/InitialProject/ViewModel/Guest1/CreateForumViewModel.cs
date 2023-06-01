using InitialProject.Model;
using InitialProject.Service;
using InitialProject.View.Guest1;
using MenuNavigation.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.ViewModel.Guest1
{
    public class CreateForumViewModel
    {
        private readonly LocationService _locationService;
        private readonly ForumService _forumService;
        public User LoggedInUser { get; set; }
        public RelayCommand CreateForumCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }


      /*  private bool _isCountrySelected;
        public bool IsCountrySelected
        {
            get { return _isCountrySelected; }
            set
            {
                _isCountrySelected = value;
                OnPropertyChanged(nameof(IsCountrySelected));
            }
        }*/

        private string _forumCountry;
        public string ForumCountry
        {
            get => _forumCountry;
            set
            {
                if (value != _forumCountry)
                {
                    _forumCountry = value;
                    OnPropertyChanged();
                  //  IsCountrySelected = !string.IsNullOrEmpty(value);
                }
            }
        }

        private string _forumCity;
        public string ForumCity
        {
            get => _forumCity;
            set
            {
                if (value != _forumCity)
                {
                    _forumCity = value;
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

        public CreateForumViewModel(ForumService forumService, LocationService locationService, User user)
        {
            _forumService = forumService;
            _locationService = locationService;
            LoggedInUser = user;

            CreateForumCommand = new RelayCommand(Execute_CreateForumCommand, CanExecute_Command);
            CancelCommand = new RelayCommand(Execute_CancelCommand, CanExecute_Command);

            InitializeCollections();
            InitializeCombobox();
        }

        private void InitializeCollections()
        {
            Countries = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
        }
        private void InitializeCombobox()
        {
            InitializeCountries();
        }
       
        private void InitializeCountries()
        {
            foreach (var country in _locationService.GetCountries().OrderBy(c => c))
            {
                Countries.Add(country);
            }
        }
        public void UpdateCityComboBox()
        {
            Cities.Clear();
            foreach (string city in _locationService.GetCitiesByCountry(ForumCountry).OrderBy(c => c))
            {
                Cities.Add(city);
            }
        }
        private Location GetLocation()
        {
            return _locationService.GetLocation(ForumCountry, ForumCity);
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public void Execute_CreateForumCommand(object obj)
        {
            var messageBoxResult = MessageBox.Show($"Are you sure you want to create new forum?", "Create Forum Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Forum forum = new Forum(GetLocation().Id, Comment, LoggedInUser.Id);
                _forumService.Save(forum);
                MessageBox.Show("Forum created successfully.");
                var window = Application.Current.Windows.OfType<CreateForum>().FirstOrDefault();
                window.Close();
            }
        }

        public void Execute_CancelCommand(object obj)
        {
            var window = Application.Current.Windows.OfType<CreateForum>().FirstOrDefault();
            window.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
