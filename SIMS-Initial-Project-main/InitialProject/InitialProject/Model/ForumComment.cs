using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class ForumComment: ISerializable
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }

        public ForumComment() { }
        public ForumComment(string comment, int userId)
        {
            Comment = comment;
            UserId = userId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Comment, UserId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Comment = values[1];
            UserId = Convert.ToInt32(values[2]);
        }
    }
}
