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
        public bool Accepted { get; set; }

        public GuideMessage()
        {
        }

        public GuideMessage(string text) 
        {
            Text = text;
            Accepted = false;
        }
    }
}
