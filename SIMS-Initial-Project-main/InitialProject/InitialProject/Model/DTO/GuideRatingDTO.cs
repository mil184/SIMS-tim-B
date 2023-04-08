using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class GuideRatingDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Checkpoint { get; set; }
        public string Comment { get; set; }
        public string Rating { get; set; }
        public string Language { get; set; }
        public string Knowledge { get; set; }
        public string Interestingness { get; set; }
        public string IsValidText { get; set; }

        public GuideRatingDTO(int id, string username, string checkpoint, string comment, int knowledge, int language, int interestingness, bool isValid)
        {
            Id = id;
            Username = username;
            Checkpoint = checkpoint;
            Comment = comment;
            Rating = getRating(knowledge,language,interestingness);
            IsValidText = getValid(isValid);
            Knowledge = knowledge.ToString();
            Language = language.ToString();
            Interestingness = interestingness.ToString();
        }

        public string getValid(bool isValid) 
        {
            if (isValid) return "Yes";
            else return "No";
        }
        public string getRating(int knowledge, int language, int interestingness) 
        {
            double finalrating = ((double)knowledge + (double)language + (double)interestingness) / 3;
            return finalrating.ToString();
        }
    }
}
