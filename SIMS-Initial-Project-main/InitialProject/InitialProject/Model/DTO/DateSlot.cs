using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class DateSlot
    {
        public DateOnly? Date { get; set; }

        public DateSlot() { }

        public DateSlot(DateOnly? date)
        {
            Date = date;
        }
    }
}
