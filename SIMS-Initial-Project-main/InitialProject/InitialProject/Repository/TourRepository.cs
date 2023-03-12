using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace InitialProject.Repository
{
    public class TourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<Tour> _serializer;

        private List<Tour> _tours;
        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(c => c.Id) + 1;
        }

        public Tour Save(Tour tour)
        {
            tour.Id = NextId();

            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tour);
            _serializer.ToCSV(FilePath, _tours);

            return tour;
        }
        public Tour GetCurrentTour()
        {
            DateTime currentDateTime = DateTime.Now;
            foreach (Tour tour in _tours)
            {
                DateTime tourStartDateTime = tour.StartTime;
                DateTime tourEndDateTime = tour.StartTime.AddHours(tour.Duration);

                if (currentDateTime >= tourStartDateTime && currentDateTime <= tourEndDateTime)
                {
                    return tour;
                }
            }
            return null;
        }

        public List<Tour> GetUpcomingTours()
        {
            List<Tour> upcomingTours = new List<Tour>();

            DateTime currentDateTime = DateTime.Now;
            foreach (Tour tour in _tours)
            {
                DateTime tourStartDateTime = tour.StartTime;
                if (tourStartDateTime > currentDateTime)
                {
                    upcomingTours.Add(tour);
                }
            }
            return upcomingTours;
        }

    }
}
