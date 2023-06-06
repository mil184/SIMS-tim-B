using System;

namespace InitialProject.Model.DTO
{
    public class AccommodationRenovationDTO
    {
        public int RenovationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String AccommodationName { get; set; }

        public AccommodationRenovationDTO() { }

        public AccommodationRenovationDTO(int renovationId, DateTime startDate, DateTime endDate, string accommodationName)
        {
            RenovationId = renovationId;
            StartDate = startDate;
            EndDate = endDate;
            AccommodationName = accommodationName;
        }
    }
}
