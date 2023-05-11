using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class Guest1RatingsDTO
    {
        public int Cleanliness { get; set; }
        public int Behavior { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }

        public Guest1RatingsDTO() { }

        public Guest1RatingsDTO(int cleanliness, int behavior, string comment, string username)  
        {
            Cleanliness = cleanliness;
            Behavior = behavior;
            Comment = comment;
            Username = username;
        }
    }
}
