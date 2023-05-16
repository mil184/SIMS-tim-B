﻿using InitialProject.Resources.Enums;
using InitialProject.Serializer;
using System;
using System.Collections.ObjectModel;

namespace InitialProject.Model
{
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public RequestStatus Status { get; set; }
        public int GuestId { get; set; }
        public int GuideId { get; set; }
        public DateTime CreationTime { get; set; }
        public TourRequest()
        {
        }

        public TourRequest(int locationId, string description, string language, int maxGuests, DateTime startTime, DateTime endTime, int guestId)
        {
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            StartTime = startTime;
            EndTime = endTime;
            Status = RequestStatus.pending;
            GuestId = guestId;
            GuideId = -1;
            CreationTime = DateTime.Now;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                LocationId.ToString(),
                Description,
                Language,
                MaxGuests.ToString(),
                StartTime.ToString(),
                EndTime.ToString(),
                Status.ToString(),
                GuestId.ToString(),
                GuideId.ToString(),
                CreationTime.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            LocationId = int.Parse(values[1]);
            Description = values[2];
            Language = values[3];
            MaxGuests = int.Parse(values[4]);
            StartTime = DateTime.Parse(values[5]);
            EndTime = DateTime.Parse(values[6]);
            Status = Enum.Parse<RequestStatus>(values[7]);
            GuestId = int.Parse(values[8]);
            GuideId = int.Parse(values[9]);
            CreationTime = DateTime.Parse(values[10]);
        }
    }
}