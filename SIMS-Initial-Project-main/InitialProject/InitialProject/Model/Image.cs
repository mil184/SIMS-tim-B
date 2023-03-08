using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Image : ISerializable
    {
        public int Id;

        public string Url;
        public Image() { }
        public Image(int id, string url)
        {
            Id = id;
            Url = url;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Url };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Url = values[1];
        }
    }
}
