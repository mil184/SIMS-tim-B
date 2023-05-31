using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Language : ISerializable
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public Language()
        {
        }
        public Language(int id, string languageName)
        {
            Id = id;
            LanguageName = languageName;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), LanguageName };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            LanguageName = values[1];
        }
    }
}
