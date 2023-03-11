using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Accommodation Accommodation { get; set; }

        public AccommodationReservation() { }

        public AccommodationReservation(int id, DateTime startDate, DateTime endDate, Accommodation accommodation)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            Accommodation = accommodation;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), StartDate.ToString(), EndDate.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartDate = Convert.ToDateTime(values[1]);
            EndDate = Convert.ToDateTime(values[2]);
        }
    }
}
