using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class Location : ISerializable
    {
        public int Id;

        public string Country;

        public string City;

        public Location() { }
        public Location(int id, string country, string city)
        {
            Id = id;
            Country = country;
            City = city;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Country, City };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Country = values[1];
            City = values[2];
        }

    }
}
