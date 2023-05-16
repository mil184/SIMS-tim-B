using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Service;
using InitialProject.ViewModel.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
        public EvaluateViewModel _viewModel { get; set; }

        public Evaluate(AccommodationRatingsDTO selectedUnratedAccommodation, AccommodationRatingService accommodationRatingsService, AccommodationReservationService accommodationReservationService, IImageRepository imageRepository)
        {
            InitializeComponent();
            _viewModel = new EvaluateViewModel(selectedUnratedAccommodation, accommodationRatingsService, accommodationReservationService, imageRepository);
            DataContext = _viewModel;
        }

    }
}
