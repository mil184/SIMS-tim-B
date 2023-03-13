using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Resources.Enums;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinReservationDays { get; set; }
        public int CancellationPeriod { get; set; }
        //public List<Image> Images { get; set; }

        public Accommodation() { }

        public Accommodation(int id, string name, int locationId, AccommodationType type, int maxGuests, int minReservationDays, int cancellationPeriod)
        {
            Id = id;
            Name = name;
            LocationId = locationId;
            Type = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
            CancellationPeriod = cancellationPeriod;
        }

        public string[] ToCSV()
        {
            //  how to save locations???
            //  how to save image list???
            string[] csvValues = { Id.ToString(), Name, LocationId.ToString(), Type.ToString(), MaxGuests.ToString(), MinReservationDays.ToString(), CancellationPeriod.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = Convert.ToInt32(values[2]);
            Type = Enum.Parse<AccommodationType>(values[3]);
            MaxGuests = Convert.ToInt32(values[4]);
            MinReservationDays = Convert.ToInt32(values[5]);
            CancellationPeriod = Convert.ToInt32(values[6]);
        }
    }
}
