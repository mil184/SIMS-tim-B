using InitialProject.Serializer;
using System;
using System.Collections.ObjectModel;

namespace InitialProject.Model
{
    public class Tour : ISerializable
    {
        public int Id;

        public string Name;

        public int LocationId;

        public string Description;

        public string Language;

        public int MaxGuests;

        public DateTime StartTime;

        public int Duration;

        public int GuideId;

      //  public ObservableCollection<Image> Images;

       // public ObservableCollection<TourPoint> TourPoints;

        public Tour() { }
        public Tour(string name, int locationId, string description, string language, int maxGuests, DateTime startTime, int duration, int guideId)
        {
            Name = name;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            StartTime = startTime;
            Duration = duration;
            GuideId = guideId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),Name, LocationId.ToString(), Description, Language, MaxGuests.ToString(), StartTime.ToString(), Duration.ToString(), GuideId.ToString() };
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
            StartTime = DateTime.Parse(values[6]);
            Duration = int.Parse(values[7]);
            GuideId = int.Parse(values[8]); 
        }
    }
}
