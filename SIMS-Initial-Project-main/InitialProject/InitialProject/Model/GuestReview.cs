using InitialProject.Serializer;
using System;

namespace InitialProject.Model
{
    public class GuestReview : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public string Comment { get; set; }
        public int Cleanness { get; set; }
        public int Behavior { get; set; }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), GuestId.ToString(), Comment, Cleanness.ToString(), Behavior.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            Comment = values[3];
            Cleanness = Convert.ToInt32(values[4]);
            Behavior = Convert.ToInt32(values[5]);
        }
    }
}
