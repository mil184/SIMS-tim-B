using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace InitialProject.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public int LocationId { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuests { get; set; }
        public int CurrentGuestCount { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public int GuideId { get; set; }

        public ObservableCollection<int> ImageIds;

        public ObservableCollection<int> CheckpointIds;

        public bool Started { get; set; }

        public Tour() { ImageIds = new ObservableCollection<int>(); CheckpointIds = new ObservableCollection<int>(); }
        public Tour(string name, int locationId, string description, string language, int maxGuests, int currentGuestCount, DateTime startTime, int duration, int guideId, ObservableCollection<int> imageIds, ObservableCollection<int> checkpointIds)
        {
            Name = name;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            CurrentGuestCount = currentGuestCount;
            StartTime = startTime;
            Duration = duration;
            GuideId = guideId;
            ImageIds = imageIds;
            CheckpointIds = checkpointIds;
            Started = false;
 
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
            Id.ToString(),
            Name,
            LocationId.ToString(),
            Description,
            Language,
            MaxGuests.ToString(),
            CurrentGuestCount.ToString(),
            StartTime.ToString(),
            Duration.ToString(),
            GuideId.ToString(),
            Started.ToString(),
            string.Join(",", ImageIds),
            string.Join(",", CheckpointIds)
        };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            LocationId = int.Parse(values[2]);
            Description = values[3];
            Language = values[4];
            MaxGuests = int.Parse(values[5]);
            CurrentGuestCount = int.Parse(values[6]);
            StartTime = DateTime.Parse(values[7]);
            Duration = int.Parse(values[8]);
            GuideId = int.Parse(values[9]);
            Started = bool.Parse(values[10]);
            foreach(string id in values[11].Split(',')) 
            {
                ImageIds.Add(int.Parse(id));
            }
            foreach (string id in values[12].Split(','))
            {
                CheckpointIds.Add(int.Parse(id));
            }

        }

    }

}
