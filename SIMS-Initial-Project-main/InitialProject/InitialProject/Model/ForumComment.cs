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
        public int ForumId { get; set; }
        public int ReportCount { get; set; }

        public ForumComment() { }
        public ForumComment(string comment, int userId, int forumId)
        {
            Comment = comment;
            UserId = userId;
            ForumId = forumId;
            ReportCount = 0;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Comment, UserId.ToString(), ForumId.ToString(), ReportCount.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Comment = values[1];
            UserId = Convert.ToInt32(values[2]);
            ForumId = Convert.ToInt32(values[3]);
            ReportCount = Convert.ToInt32(values[4]);
        }
    }
}
