using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class GuideTourDTO
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime StartTime { get; set; }

        public GuideTourDTO()
        {

        }
        public GuideTourDTO(int id, string name, string country, string city, DateTime startTime)
        {
            Id = id;
            Name = name;
            Country = country;
            City = city;
            StartTime = startTime;
        }
    }
}
