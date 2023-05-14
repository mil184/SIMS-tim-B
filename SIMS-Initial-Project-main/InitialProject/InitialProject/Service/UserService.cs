using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Enums;
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
        private readonly UserRepository _userRepository;
        private readonly AccommodationReservationService _accommodationReservationService;

        public UserService()
        {
            _userRepository = new UserRepository();
            _accommodationReservationService = new AccommodationReservationService();
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
