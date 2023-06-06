using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class ComplexTourRequestDTO
    {
        public int Id { get; set; }
        public string Locations { get; set; }
        public string Languages { get; set; }
        public string StartDates { get; set; }
        public string EndDates { get; set; }
        public string Statuses { get; set; }
        public string ChosenDates { get; set; }
        public string Status { get; set; }

        public ComplexTourRequestDTO() { }

        public ComplexTourRequestDTO(int id, string locations, string languages, string startDates, string endDates, string statuses, string chosenDates, string status)
        {
            Id = id;
            Locations = locations;
            Languages = languages;
            StartDates = startDates;
            EndDates = endDates;
            Statuses = statuses;
            ChosenDates = chosenDates;
            Status = status;
        }
    }
}
