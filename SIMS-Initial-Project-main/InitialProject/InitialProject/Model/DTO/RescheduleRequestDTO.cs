using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InitialProject.Model.DTO
{
    public class RescheduleRequestDTO
    {
        public int RescheduleRequestId { get; set; }
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public bool IsAvailable { get; set; }

        public RescheduleRequestDTO() { }

        public RescheduleRequestDTO(int rescheduleRequestId, int accommodationId, string accommodationName, DateTime newStartDate, DateTime newEndDate)
        {
            RescheduleRequestId = rescheduleRequestId;
            AccommodationId = accommodationId;
            AccommodationName = accommodationName;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
        }

        public ImageSource TypeImage
        {
            get
            {
                string imagePath;
                if (IsAvailable)
                {
                    imagePath = "/Resources/Images/OwnerIcons/check.png";
                }
                else
                {
                    imagePath = "/Resources/Images/OwnerIcons/cancel.png";
                }

                return new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
        }
    }
}
