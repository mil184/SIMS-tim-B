﻿using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class AccommodationRatings: ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int AccommodationId { get; set; }
        public int OwnerId { get; set; }
        public int Cleanliness { get; set; }
        public int Correctness { get; set; }
        public string Comment { get; set; }

        public AccommodationRatings() { }

        public AccommodationRatings(int id, int reservationId, int accommodationId, int ownerId, int cleanliness, int correctness, string comment) 
        {
            Id = id;
            ReservationId = reservationId;
            AccommodationId = accommodationId;
            OwnerId = ownerId;
            Cleanliness = cleanliness;
            Correctness = correctness;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), AccommodationId.ToString(), OwnerId.ToString(), Cleanliness.ToString(), Correctness.ToString(), Comment };
            return csvValues;
        }
       
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            AccommodationId = Convert.ToInt32(values[2]);
            OwnerId = Convert.ToInt32(values[3]);
            Cleanliness = Convert.ToInt32(values[4]);
            Correctness = Convert.ToInt32(values[5]);
            Comment = values[6];
        }
    }
}
