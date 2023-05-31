using InitialProject.Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class AvailableAccommodationsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public AvailableAccommodationsDTO()
        {

        }
        public AvailableAccommodationsDTO(int id, string name, string country, string city)
        {
            Id = id;
            Name = name;
            Country = country;
            City = city;
        }
    }
}
