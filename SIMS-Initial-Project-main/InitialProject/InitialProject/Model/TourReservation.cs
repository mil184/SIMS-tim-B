using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Resources.Enums;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    internal class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public int PersonCount { get; set; }
        public TourReservation() { }

        public TourReservation(int userId, int tourId, int personCount)
        {
            UserId = userId;
            TourId = tourId;
            PersonCount = personCount;
            
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), TourId.ToString(), PersonCount.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TourId = Convert.ToInt32(values[2]);
            PersonCount = Convert.ToInt32(values[3]);
  
        }
    }
}
