using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class RequestFilterParameters
    {
        public string? Country;
        public string? City;
        public string? Language;
        public int? MaxGuests;
        public DateTime? StartDate;
        public DateTime? EndDate;

        public RequestFilterParameters() { }
        public RequestFilterParameters(string? country, string? city, string? language, int? maxGuests, DateTime? startTime, DateTime? endTime)
        {
            Country = country;
            City = city;
            Language = language;
            MaxGuests = maxGuests;
            StartDate = startTime;
            EndDate = endTime;
        }
    }
}
