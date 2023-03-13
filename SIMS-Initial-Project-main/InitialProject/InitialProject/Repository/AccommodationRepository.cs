﻿using InitialProject.Model;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    class AccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(c => c.Id) + 1;
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();

            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath, _accommodations);

            return accommodation;
        }
    }
}