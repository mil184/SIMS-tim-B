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

       // public DateTime StartTime;

        public int Duration;

        public int GuideId;

      //  public ObservableCollection<Image> Images;

       // public ObservableCollection<TourPoint> TourPoints;

        public Tour() { }
        public Tour(string name, string description, string language, int maxGuests, int duration)
        {
            Name = name;
            Description = description;
            Language = language;
            MaxGuests = maxGuests;
            Duration = duration;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),Name, Description, Language, MaxGuests.ToString(),Duration.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Description = values[2];
            Language = values[3];
            MaxGuests = int.Parse(values[4]);
            Duration = int.Parse(values[5]);
        }
    }
}
