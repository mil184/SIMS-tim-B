using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

        public ObservableCollection<Image> Images;

        public ObservableCollection<TourPoint> TourPoints;

        public string[] ToCSV()
        {
            throw new NotImplementedException();
        }

        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }
    }
}
