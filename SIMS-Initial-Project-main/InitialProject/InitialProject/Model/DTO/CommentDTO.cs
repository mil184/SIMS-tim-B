using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public int ForumId { get; set; }
        public bool WasPresent { get; set; }

        public CommentDTO() { }
        public CommentDTO(int id, string comment, string username, int forumId, bool wasPresent)
        {
            Id = id;
            Comment = comment;
            Username = username;
            ForumId = forumId;
            WasPresent = wasPresent;
        }
    }
}
