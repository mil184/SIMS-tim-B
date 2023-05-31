using InitialProject.Serializer;
using System;

namespace InitialProject.Model
{
    public class AccommodationRenovation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int OwnerId { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public AccommodationRenovation() { }

        public AccommodationRenovation(int accommodationId, int ownerId, int duration, DateTime startDate, DateTime endDate, string description)
        {
            AccommodationId = accommodationId;
            OwnerId = ownerId;
            Duration = duration;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), OwnerId.ToString(), Duration.ToString(), StartDate.ToString(), EndDate.ToString(), Description };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            OwnerId = Convert.ToInt32(values[2]);
            Duration = Convert.ToInt32(values[3]);
            StartDate = Convert.ToDateTime(values[4]);
            EndDate = Convert.ToDateTime(values[5]);
            Description = values[6];
        }
    }
}
