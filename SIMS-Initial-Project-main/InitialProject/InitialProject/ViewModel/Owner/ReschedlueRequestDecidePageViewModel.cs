using InitialProject.Model;
using InitialProject.Service;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;

namespace InitialProject.ViewModel.Owner
{
    public class ReschedlueRequestDecidePageViewModel : INotifyPropertyChanged
    {
        public NavigationService navigationService { get; set; }
        public RescheduleRequest Request { get; set; }

        private readonly RescheduleRequestService _service;

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

        private string _color;
        public string Color
        {
            get => _color;
            set
            {
                if (value != _color)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _availability;
        public string Availability
        {
            get => _availability;
            set
            {
                if (value != _availability)
                {
                    _availability = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReschedlueRequestDecidePageViewModel(NavigationService navService, RescheduleRequestService service, int requestId, bool isAvailable)
        {
            navigationService = navService;
            _service = service;
            Request = _service.GetById(requestId);

            if (isAvailable == true)
            {
                Color = "green";
                Availability = "available";
            }
            else
            {
                Color = "red";
                Availability = "unavailable";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
