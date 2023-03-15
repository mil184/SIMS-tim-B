using InitialProject.Serializer;
using System;
using System.Collections.Generic;

namespace InitialProject.Model.DTO
{
    public class Guest2TourDTO 
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public int CurrentGuestCount { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Guide { get; set; }

        //public list<string> images;
        //public list<int> checkpointids;
        //public list<int> checkpointnames;

        public Guest2TourDTO()
        {
        }

        public Guest2TourDTO(string name, string country, string city, string description, string language, int maxGuests, int currentGuestCount, DateTime startTime, int duration, string guide)
        {   
            Name = name;
            Country = country;
            City = city;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            CurrentGuestCount = currentGuestCount;
            StartTime = startTime;
            Duration = duration;
            Guide = guide;
            //this.images = images;
            //this.checkpointIds = checkpointIds;
            //this.checkpointNames = checkpointNames;
        }
    }
}
