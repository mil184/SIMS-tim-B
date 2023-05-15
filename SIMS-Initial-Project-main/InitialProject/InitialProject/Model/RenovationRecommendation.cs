using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class RenovationRecommendation: ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public string Information { get; set; }
        public int RenovationLevel { get; set; }
        public int GuestId { get; set; }
        public DateTime RecommendationDate { get; set; }

        public RenovationRecommendation() { }
        public RenovationRecommendation(/*int id,*/ int accommodationId, string information, int renovationLevel, int guestId, DateTime recommendationDate)
        {
            // Id = id;
            AccommodationId = accommodationId;
            Information = information;
            RenovationLevel = renovationLevel;
            GuestId = guestId;
            RecommendationDate = recommendationDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString() ,Information, RenovationLevel.ToString(), GuestId.ToString(), RecommendationDate.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            Information = values[2];
            RenovationLevel = Convert.ToInt32(values[3]);
            GuestId = Convert.ToInt32(values[4]);
            RecommendationDate = Convert.ToDateTime(values[5]);
        }
    }
}
