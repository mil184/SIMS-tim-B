using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class ForumDTO
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public bool IsOpened { get; set; }
        public bool IsVeryUseful { get; set; }

        public ForumDTO() {}
        public ForumDTO(int id, string country, string city, string comment, string username, bool isOpened, bool isVeryUseful)
        {
            Id = id;
            Country = country;
            City = city;
            Comment = comment;
            Username = username;
            IsOpened = isOpened;
            IsVeryUseful = isVeryUseful;
        }
    }
}
