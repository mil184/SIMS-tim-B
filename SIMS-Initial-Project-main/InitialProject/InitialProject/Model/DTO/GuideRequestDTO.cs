using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class GuideRequestDTO
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }
        public string MaximumGuests { get; set; }
        public string Interval { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }

        public GuideRequestDTO(int id, string location, string language, string maximumGuests, DateTime startTime, DateTime endTime, string description)
        {
            Id = id;
            Location = location;
            Language = language;
            MaximumGuests = maximumGuests;
            StartTime = startTime;
            EndTime = endTime;
            Interval =
                StartTime.Day.ToString() + "." +
                StartTime.Month.ToString() + "." +
                StartTime.Year.ToString() + "." + " - " +

                EndTime.Day.ToString() + "." +
                EndTime.Month.ToString() + "." +
                EndTime.Year.ToString() + ".";

            Description = description;
        }
    }
}
