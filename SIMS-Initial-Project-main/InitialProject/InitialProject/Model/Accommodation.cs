using InitialProject.Resources.Enums;
using InitialProject.Serializer;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace InitialProject.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinReservationDays { get; set; }
        public int CancellationPeriod { get; set; }
        public bool IsRenovated { get; set; }
        public ObservableCollection<int> ImageIds = new ObservableCollection<int>();

        public Accommodation() { }

        public Accommodation(string name, int ownerId, int locationId, AccommodationType type, int maxGuests, int minReservationDays, int cancellationPeriod, ObservableCollection<int> imageIds)
        {
            Name = name;
            OwnerId = ownerId;
            LocationId = locationId;
            Type = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
            CancellationPeriod = cancellationPeriod;
            IsRenovated = false;
            ImageIds = imageIds;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), OwnerId.ToString(), Name, LocationId.ToString(), Type.ToString(), MaxGuests.ToString(), MinReservationDays.ToString(), CancellationPeriod.ToString(), IsRenovated.ToString(), string.Join(",", ImageIds) };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            Name = values[2];
            LocationId = Convert.ToInt32(values[3]);
            Type = Enum.Parse<AccommodationType>(values[4]);
            MaxGuests = Convert.ToInt32(values[5]);
            MinReservationDays = Convert.ToInt32(values[6]);
            CancellationPeriod = Convert.ToInt32(values[7]);
            IsRenovated = Convert.ToBoolean(values[8]);
            foreach (string id in values[9].Split(','))
            {
                ImageIds.Add(int.Parse(id));
            }
        }

        public ImageSource TypeImage
        {
            get
            {
                string imagePath;
                if (Type == AccommodationType.House)
                {
                    imagePath = "/Resources/Images/OwnerIcons/house.png";
                }
                else if (Type == AccommodationType.Hut)
                {
                    imagePath = "/Resources/Images/OwnerIcons/hut.png";
                }
                else
                {
                    imagePath = "/Resources/Images/OwnerIcons/apartment.png";
                }

                return new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
        }
    }
}
