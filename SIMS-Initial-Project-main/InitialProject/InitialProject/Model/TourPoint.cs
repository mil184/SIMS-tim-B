using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class TourPoint
    {
        public int Id;

        public string Name;

        public int TourId;

        public int Order;

        public bool IsActive;

        public TourPoint() { }
        public TourPoint(int id, string name, int tourId, int order, bool isActive)
        {
            Id = id;
            Name = name;
            TourId = tourId;
            Order = order;
            IsActive = isActive;
        }

        public string[] ToCSV()
        {

            string[] csvValues = { Id.ToString(), Name, TourId.ToString(), Order.ToString(), IsActive.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            TourId = Convert.ToInt32(values[2]);
        }

    }
}
