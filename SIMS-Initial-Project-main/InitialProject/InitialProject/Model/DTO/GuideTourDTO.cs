using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class GuideTourDTO
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime StartTime { get; set; }

        public GuideTourDTO()
        {

        }
        public GuideTourDTO(string name, string country, string city, DateTime startTime)
        {
            Name = name;
            Country = country;
            City = city;
            StartTime = startTime;
        }
    }
}
