using InitialProject.Serializer;
using System;

namespace InitialProject.Model
{
    public class ReservationCancellation : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int AccommodationId { get; set; }
        public int OwnerId { get; set; }
        public int GuestId { get; set; }
        public DateTime CancellationDate { get; set; }

        public ReservationCancellation() { }

        public ReservationCancellation(int reservationId, int accommodationId, int ownerId, int guestId, DateTime cancellationDate)
        {
            ReservationId = reservationId;
            AccommodationId = accommodationId;
            OwnerId = ownerId;
            GuestId = guestId;
            CancellationDate = cancellationDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), AccommodationId.ToString(), OwnerId.ToString(), GuestId.ToString(), CancellationDate.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            OwnerId = Convert.ToInt32(values[3]);
            GuestId = Convert.ToInt32(values[4]);
            CancellationDate = Convert.ToDateTime(values[5]);
        }
    }
}
