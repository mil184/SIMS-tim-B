using InitialProject.Service;
using InitialProject.ViewModel.Owner;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.View.Owner
{
    public partial class RescheduleRequestDecidePage : Page
    {
        public ReschedlueRequestDecidePageViewModel _viewModel { get; set; }
        public NavigationService navigationService { get; set; }
        public RescheduleRequestService _service { get; set; }
        public int RequestId { get; set; }
        public bool IsAvailable { get; set; }

        public RescheduleRequestDecidePage(NavigationService navService, RescheduleRequestService service, int requestId, bool isAvailable)
        {
            InitializeComponent();
            navigationService = navService;
            _service = service;
            RequestId = requestId;
            IsAvailable = isAvailable;
            _viewModel = new ReschedlueRequestDecidePageViewModel(navigationService, _service, RequestId, IsAvailable);
            DataContext = _viewModel;
        }
    }
}
