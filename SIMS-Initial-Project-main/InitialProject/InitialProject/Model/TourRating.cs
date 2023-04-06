using InitialProject.Serializer;
using System;
using System.Collections.ObjectModel;

namespace InitialProject.Model
{
    public class TourRating : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideLanguage { get; set; }
        public int Interestingness { get; set; }
        public String Comment { get; set; }

        public ObservableCollection<int> ImageIds;

        public TourRating() { ImageIds = new ObservableCollection<int>(); }

        public TourRating(int tourId, int guideKnowledge, int guideLanguage, int interestingness, string comment, ObservableCollection<int> imageIds)
        {
            TourId = tourId;
            GuideKnowledge = guideKnowledge;
            GuideLanguage = guideLanguage;
            Interestingness = interestingness;
            Comment = comment;
            ImageIds = imageIds;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
                {   Id.ToString(),
                    TourId.ToString(),
                    GuideKnowledge.ToString(),
                    GuideLanguage.ToString(),
                    Interestingness.ToString(),
                    Comment,
                    string.Join(",", ImageIds) };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            GuideKnowledge = Convert.ToInt32(values[2]);
            GuideLanguage = Convert.ToInt32(values[3]);
            Interestingness = Convert.ToInt32(values[4]);
            Comment = values[5];
            foreach (string id in values[6].Split(','))
            {
                ImageIds.Add(int.Parse(id));
            }
        }
    }
}
