using InitialProject.Resources.Enums;
using InitialProject.Serializer;
using iTextSharp.text;
using System;
using System.Collections.Generic;

namespace InitialProject.Model
{
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
        public int? NumberOfReservations { get; set; }
        public int? BonusPoints { get; set; }
        public DateTime? SuperGuestExpirationDate { get; set; }

        public List<string> SuperGuideLanguages;
        public User() { }

        public User(string username, string password, UserType type, int? numberOfReservations, int? bonusPoints, DateTime? superGuestExpirationDate)
        {
            Username = username;
            Password = password;
            Type = type;
            NumberOfReservations = numberOfReservations;
            BonusPoints = bonusPoints;
            SuperGuestExpirationDate = superGuestExpirationDate;
            SuperGuideLanguages = new List<string>();
        }

        public string[] ToCSV()
        {
            string numberOfReservations = NumberOfReservations.HasValue ? NumberOfReservations.Value.ToString() : "";
            string bonusPoints = BonusPoints.HasValue ? BonusPoints.Value.ToString() : "";
            string superGuestExpirationDate = SuperGuestExpirationDate.HasValue ? SuperGuestExpirationDate.Value.ToString() : "";

            string superGuideLanguages = "";
            if(superGuideLanguages != null && Type==UserType.guide) 
            {
                string.Join(",", superGuideLanguages);
            }

            string[] csvValues = { Id.ToString(), Username, Password, Type.ToString(), numberOfReservations, bonusPoints, superGuestExpirationDate,superGuideLanguages };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Type = Enum.Parse<UserType>(values[3]);
            NumberOfReservations = string.IsNullOrEmpty(values[4]) ? null : (int?)Convert.ToInt32(values[4]);
            BonusPoints = string.IsNullOrEmpty(values[5]) ? null : (int?)Convert.ToInt32(values[5]);
            SuperGuestExpirationDate = string.IsNullOrEmpty(values[6]) ? null : (DateTime?)Convert.ToDateTime(values[6]);

            if (string.IsNullOrEmpty(values[7])) 
            {
                SuperGuideLanguages = new List<string>();
            }
            else 
            {
                SuperGuideLanguages = new List<string>();
                foreach (string language in values[7].Split(','))
                {
                    SuperGuideLanguages.Add(language);
                }
            }
        }
    }
}
