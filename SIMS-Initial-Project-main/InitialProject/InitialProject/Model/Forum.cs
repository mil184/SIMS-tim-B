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
        public int UserId { get; set; }
        public bool IsOpened { get; set; }
        public bool IsVeryUseful { get; set; }
        public int GuestCommentsCount { get; set; }
        public int OwnerCommentsCount { get; set; }

        public Forum() { }

        public Forum(int locationId, string comment, int userId)
        {
            LocationId = locationId;
            Comment = comment;
            UserId = userId;
            IsOpened = true;
            IsVeryUseful = false;
            GuestCommentsCount = 0;
            OwnerCommentsCount = 0;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), LocationId.ToString(), Comment, UserId.ToString(), IsOpened.ToString(), IsVeryUseful.ToString(), GuestCommentsCount.ToString(), OwnerCommentsCount.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            LocationId = Convert.ToInt32(values[1]);
            Comment = values[2];
            UserId = Convert.ToInt32(values[3]);
            IsOpened = Convert.ToBoolean(values[4]);
            IsVeryUseful = Convert.ToBoolean(values[5]);
            GuestCommentsCount = Convert.ToInt32(values[6]);
            OwnerCommentsCount = Convert.ToInt32(values[7]);
        }

        public ImageSource TypeImage
        {
            get
            {
                string imagePath;
                if (GuestCommentsCount >= 20 && OwnerCommentsCount >= 10)
                {
                    imagePath = "/Resources/Images/OwnerIcons/star.png";
                }
                else
                {
                    imagePath = "";
                }

                return new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
        }
    }
}
