using InitialProject.Resources.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Comment { get; set; }
        public int GuestId { get; set; }
        public bool IsOpened { get; set; }

        public Forum() { }

        public Forum(int locationId, string comment, int guestId)
        {
            LocationId = locationId;
            Comment = comment;
            GuestId = guestId;
            IsOpened = true;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), LocationId.ToString(), Comment, GuestId.ToString(), IsOpened.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            LocationId = Convert.ToInt32(values[1]);
            Comment = values[2];
            GuestId = Convert.ToInt32(values[3]);
            IsOpened = Convert.ToBoolean(values[4]);
        }
    }
}
