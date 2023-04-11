﻿using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Service;

namespace InitialProject.ViewModel.Guide
{
    public class RatingsViewModel : INotifyPropertyChanged, IObserver
    {
        private readonly UserRepository _userRepository;
        private readonly TourRatingRepository _tourRatingRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly CheckpointService _checkpointService;
        private readonly Tour _finishedTour;

        private ObservableCollection<GuideRatingDTO> _guestRatings;
        public ObservableCollection<GuideRatingDTO> GuestRatings
        {
            get => _guestRatings;
            set
            {
                _guestRatings = value;
                OnPropertyChanged();
            }
        }

        private GuideRatingDTO _selectedRatingDTO;
        public GuideRatingDTO SelectedRatingDTO
        {
            get => _selectedRatingDTO;
            set
            {
                _selectedRatingDTO = value;
                OnPropertyChanged();
            }
        }

        public RatingsViewModel(UserRepository userRepository, TourRatingRepository tourRatingRepository, TourReservationRepository tourReservationRepository, CheckpointService checkpointService, Tour finishedTour)
        {
            _userRepository = userRepository;
            _tourRatingRepository = tourRatingRepository;
            _tourReservationRepository = tourReservationRepository;
            _checkpointService = checkpointService;
            _finishedTour = finishedTour;

            _tourRatingRepository.Subscribe(this);

            GuestRatings = new ObservableCollection<GuideRatingDTO>(ConvertToDTO(_tourRatingRepository.GetTourRatings(_finishedTour)));
        }

        public GuideRatingDTO ConvertToDTO(TourRating rating)
        {
            if (rating == null)
                return null;

            string checkpointName = GetCheckpointName(rating.UserId, rating.TourId);
            string username = GetUserName(rating.UserId);
            List<string> ratingUrls = GetRatingUrls(rating);

            return BuildGuideRatingDTO(rating, checkpointName, username, ratingUrls);
        }

        private string GetCheckpointName(int userId, int tourId)
        {
            var reservation = _tourReservationRepository.GetReservationByGuestIdAndTourId(userId, tourId);
            if (reservation.CheckpointArrivalId == -1)
            {
                return "Did not arrive";
            }
            else
            {
                var checkpoint = _checkpointService.GetById(reservation.CheckpointArrivalId);
                return BuildCheckpointName(checkpoint.Order, checkpoint.Name);
            }
        }

        private string BuildCheckpointName(int order, string name)
        {
            return $"{order}. {name}";
        }

        private string GetUserName(int userId)
        {
            var user = _userRepository.GetById(userId);
            return user.Username;
        }

        private List<string> GetRatingUrls(TourRating rating)
        {
            return _tourRatingRepository.FindRatingUrls(rating);
        }

        private GuideRatingDTO BuildGuideRatingDTO(TourRating rating, string checkpointName, string username, List<string> ratingUrls)
        {
            return new GuideRatingDTO(
                rating.Id,
                username,
                checkpointName,
                rating.Comment,
                rating.GuideKnowledge,
                rating.GuideLanguage,
                rating.Interestingness,
                rating.isValid,
                ratingUrls);
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

        public void Update()
        {
            GuestRatings.Clear();
            GuestRatings = new ObservableCollection<GuideRatingDTO>(ConvertToDTO(_tourRatingRepository.GetTourRatings(_finishedTour)));
        }

        public void ReportTourRating()
        {
            if (SelectedRatingDTO == null)
                return;

            TourRating rating = ConvertToRating(SelectedRatingDTO);
            rating.isValid = false;
            _tourRatingRepository.Update(rating);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
