using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class GuideMessage
    {
        public string Text { get; set; }
        public string Language { get; set; }

        public bool Got { get; set; }
        public bool Accepted { get; set; }

        public GuideMessage()
        {
        }

        public GuideMessage(string language, bool got) 
        {
            Language = language;
            Accepted = false;
            Got = got;

            if (Got) 
            {
                Text = "You have been promoted to SUPERGUIDE for language: " + Language;
            }
            if (!Got)
            {
                Text = "You have been demoted to GUIDE for language: " + Language;
            }
        }
    }
}
