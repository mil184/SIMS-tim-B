﻿using System;
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
        public Location Location { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinReservationDays { get; set; }
        public int CancellationPeriod { get; set; }
        public List<Image> Images { get; set; }

        public Accommodation(int id, string name, Location location, AccommodationType type, int maxGuests, int minReservationDays, int cancellationPeriod, List<Image> images)
        {
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            MaxGuests = maxGuests;
            MinReservationDays = minReservationDays;
            CancellationPeriod = cancellationPeriod;
            Images = images;
        }

        public string[] ToCSV()
        {
            //  how to save locations???
            //  how to save image list???
            string[] csvValues = { Id.ToString(), Name, Location.City, Location.Country, Type.ToString(), MaxGuests.ToString(), MinReservationDays.ToString(), CancellationPeriod.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location.City = values[2];
            Location.Country = values[3];
            Type = Enum.Parse<AccommodationType>(values[4]);
            MaxGuests = Convert.ToInt32(values[5]);
            MinReservationDays = Convert.ToInt32(values[6]);
            CancellationPeriod = Convert.ToInt32(values[7]);
        }
    }
}
