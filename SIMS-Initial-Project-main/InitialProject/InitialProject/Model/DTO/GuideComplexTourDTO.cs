using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class GuideComplexTourDTO
    {
        public int Id { get; set; }
        public string Locations { get; set; }
        public string Languages { get; set; }
        public string NumberOfGuests { get; set; }

        public GuideComplexTourDTO() { }

        public GuideComplexTourDTO(int id, string locations, string languages, string numberOfGuests)
        {
            Id = id;
            Locations = locations;
            Languages = languages;
            NumberOfGuests = numberOfGuests;
        }

    }
}
