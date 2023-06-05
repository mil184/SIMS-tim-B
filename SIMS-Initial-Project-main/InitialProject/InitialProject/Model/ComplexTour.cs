using InitialProject.Resources.Enums;
using InitialProject.Serializer;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Model
{
    public class ComplexTour : ISerializable
    {
        public int Id { get; set; }
        public List<int> AvailableTourRequestIds { get; set; }
        public Dictionary<int,int> AcceptedTourIdsByGuideIds { get; set; }
        public ComplexTourStatus Status { get; set; }

        public ComplexTour() 
        {
            AvailableTourRequestIds = new List<int>();
            AcceptedTourIdsByGuideIds = new Dictionary<int, int>();
            Status = ComplexTourStatus.pending;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {

                Id.ToString(),

                string.Join(",", AvailableTourRequestIds),

                string.Join(",", AcceptedTourIdsByGuideIds
                        .Select(entry => $"{entry.Key}:{entry.Value}")
                        .ToArray()),
                Status.ToString()

        };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);

            foreach (string id in values[1].Split(','))
            {
                AvailableTourRequestIds.Add(int.Parse(id));
            }

            string acceptedTourIdsString = values[2];
            if (!string.IsNullOrEmpty(acceptedTourIdsString))
            {
                foreach (string element in acceptedTourIdsString.Split(','))
                {
                    string[] parts = element.Split(":");
                    if (parts.Length == 2)
                    {
                        int guideId = int.Parse(parts[0]);
                        int tourId = int.Parse(parts[1]);

                        AcceptedTourIdsByGuideIds.Add(guideId, tourId);
                    }
                }
            }

            Status = Enum.Parse<ComplexTourStatus>(values[3]);
        }

    }
}
