using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.ViewModel.Guest1;
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
    /// Interaction logic for SendRequest.xaml
    /// </summary>
    public partial class SendRequest : Window 
    {
        private readonly SendRequestViewModel _viewModel;

        public SendRequest(AccommodationReservation selectedReservation, RescheduleRequestService rescheduleRequestService)
        {
            InitializeComponent();
            _viewModel = new SendRequestViewModel(selectedReservation, rescheduleRequestService);
            DataContext = _viewModel;
        }
      
    }
}
