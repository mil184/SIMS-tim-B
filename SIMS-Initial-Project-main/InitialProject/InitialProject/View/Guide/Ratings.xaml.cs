using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace InitialProject.View.Guide
{
    /// <summary>
    /// Interaction logic for Ratings.xaml
    /// </summary>
    public partial class Ratings : Window
    {
        private readonly UserRepository _userRepository;
        private readonly TourRatingRepository _tourRatingRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly CheckpointRepository _checkpointRepository;

        public Tour FinishedTour { get; set; }

        public ObservableCollection<GuideRatingDTO> GuestRatings { get; set; } 

        public GuideRatingDTO SelectedRatingDTO { get; set; }
        public Ratings(Tour finishedTour, UserRepository userRepository, TourRatingRepository tourRatingRepository, TourReservationRepository tourReservationRepository, CheckpointRepository checkpointRepository)
        {
            InitializeComponent();
            DataContext = this;
            FinishedTour = finishedTour;

            _userRepository = userRepository;
            _tourRatingRepository = tourRatingRepository;
            _tourReservationRepository = tourReservationRepository;
            _checkpointRepository = checkpointRepository;

            GuestRatings = new ObservableCollection<GuideRatingDTO>(ConvertToDTO(_tourRatingRepository.GetTourRatings(finishedTour)));

        }
        public GuideRatingDTO ConvertToDTO(TourRating rating)
        {

            if (rating == null)
                return null;

            return new GuideRatingDTO(
                    rating.Id,
                    _userRepository.GetById(rating.UserId).Username,
                    _checkpointRepository.GetById(_tourReservationRepository.GetReservationByGuestIdAndTourId(rating.UserId, rating.TourId).CheckpointArrivalId).Name,
                    rating.Comment,
                    rating.GuideKnowledge,
                    rating.GuideLanguage,
                    rating.Interestingness,
                    rating.isValid);
        }
        public List<GuideRatingDTO> ConvertToDTO(List<TourRating> ratings)
        {
            List<GuideRatingDTO> dtos = new List<GuideRatingDTO>();

            foreach (TourRating rating in ratings)
            {
                dtos.Add(ConvertToDTO(rating));
            }
            return dtos;
        }
        public TourRating ConvertToRating(GuideRatingDTO dto)
        {
            if (dto != null)
                return _tourRatingRepository.GetById(dto.Id);
            return null;
        }
        private void RatingsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RatingsOverview overview = new RatingsOverview(SelectedRatingDTO);
            overview.Show();
        }
    }
}
