using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Repository.Interfaces;
using InitialProject.Resources.Enums;
using InitialProject.Resources.Injector;
using InitialProject.Resources.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Service 
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly TourService _tourService;

        public UserService()
        {
            _userRepository = Injector.CreateInstance<IUserRepository>();
            _accommodationReservationService = new AccommodationReservationService();
            _tourService = new TourService();
        }

        public User GetByUsername(string username)
        {
            return _userRepository.GetAll().FirstOrDefault(u => u.Username == username);
        }

        public void PromoteToSuperGuest(int id)
        {
            int numberOfReservations = _accommodationReservationService.GetAll().Where(i => i.GuestId == id).Count();
            User guest = _userRepository.GetById(id);

            if (numberOfReservations >= 10 && guest.Type == UserType.guest1)
            {
                guest.Type = UserType.superguest;
                guest.NumberOfReservations = numberOfReservations;
                guest.BonusPoints = 5;
                guest.SuperGuestExpirationDate = DateTime.Now.AddDays(365);

                _userRepository.Update(guest);
            }
            else if (guest.Type == UserType.superguest && guest.SuperGuestExpirationDate <= DateTime.Now && numberOfReservations < 10)
            {
                guest.Type = UserType.guest1;
                guest.SuperGuestExpirationDate = null;
                guest.BonusPoints = null;
                guest.NumberOfReservations = numberOfReservations;

                _userRepository.Update(guest);
            }
        }

        public bool ShouldMessageBeShown(User user, List<string> languages)
        {
            return languages.SequenceEqual(user.SuperGuideLanguages);
        }
        public List<string> UpdateSuperGuidesLanguagesGot(User user)
        {
            List<string> updatedLanguages = new List<string>();

                List<string> currentLanguages = user.SuperGuideLanguages;
                List<string> newLanguages = QualifiesForSuperGuide(user);

                // Find the languages in newLanguages that are not in currentLanguages
                List<string> addedLanguages = newLanguages.Except(currentLanguages).ToList();

                // Add the addedLanguages to the updatedLanguages list
                updatedLanguages.AddRange(addedLanguages);


            return updatedLanguages;
        }

        public List<string> UpdateSuperGuidesLanguagesLost(User user)
        {
            List<string> lostLanguages = new List<string>();


                List<string> currentLanguages = user.SuperGuideLanguages;
                List<string> newLanguages = QualifiesForSuperGuide(user);

                // Find the languages in currentLanguages that are not in newLanguages
                List<string> lost = currentLanguages.Except(newLanguages).ToList();

                // Add the lost languages to the lostLanguages list
                lostLanguages.AddRange(lost);
    

            return lostLanguages;
        }

        public List<string> QualifiesForSuperGuide(User user)
        {
            List<string> superLanguages = new List<string>();

            foreach(string language in _tourService.GetAllLanguages()) 
            {
                List<Tour> tours = _tourService.GetLastYearsToursByLanguage(user, language);

                if(tours.Count >= 20 && _tourService.GetAverageLanguageRating(tours) > 4.0) 
                {
                    superLanguages.Add(language);
                }

            }
            return superLanguages;
        }
        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public void Subscribe(IObserver observer)
        {
            _userRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _userRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _userRepository.NotifyObservers();
        }

        public User Save(User user)
        {
            return _userRepository.Save(user);
        }

        public User Update(User user)
        {
            return _userRepository.Update(user);
        }

        internal List<User> GetAll()
        {
            return _userRepository.GetAll();
        }
    }
}
