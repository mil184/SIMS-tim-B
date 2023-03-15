using InitialProject.Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public  class GuestAccommodationDTO
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinReservationDays { get; set; }

        public GuestAccommodationDTO()
        {

        }
        public GuestAccommodationDTO(string name, string country, string city, AccommodationType type, int maxGuests, int minReservationDays)
        {
            Name = name;
            Country = country;
            City = city;
            Type = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
        }
    }
}
