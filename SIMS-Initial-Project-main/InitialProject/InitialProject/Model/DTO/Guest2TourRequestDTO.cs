using InitialProject.Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class Guest2TourRequestDTO
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public RequestStatus Status { get; set; }
       
        public Guest2TourRequestDTO()
        {

        }

        public Guest2TourRequestDTO(int id, string country, string city, string description, string language, int maxGuests, DateTime startTime, DateTime endTime, RequestStatus status)
        {
            Id = id;
            Country = country;
            City = city;
            Location = city + ", " + country;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
        }
    }
}
