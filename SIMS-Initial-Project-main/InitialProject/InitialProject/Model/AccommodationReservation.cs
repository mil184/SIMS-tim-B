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
        public int GuestId { get; set; }
        public int AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberDays { get; set; }
        public bool IsAvailable { get; set; }

        public AccommodationReservation() { }
        public AccommodationReservation( int guestId, int accommodationId, DateTime startDate, DateTime endDate, int numberDays)
        {
            GuestId = guestId;
            AccommodationId = accommodationId;
            StartDate = startDate;
            EndDate = endDate;
            NumberDays = numberDays;
            IsAvailable = false;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), AccommodationId.ToString(), StartDate.ToString(), EndDate.ToString(), NumberDays.ToString(), IsAvailable.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            StartDate = Convert.ToDateTime(values[3]);
            EndDate = Convert.ToDateTime(values[4]);
            NumberDays = Convert.ToInt32(values[5]);
            IsAvailable = Convert.ToBoolean(values[6]);
        }
    }
}
