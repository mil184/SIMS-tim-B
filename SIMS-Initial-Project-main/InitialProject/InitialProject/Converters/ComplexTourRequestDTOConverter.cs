using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Converters
{
    public class ComplexTourRequestDTOConverter
    {
        static public ComplexTourRequestDTO ConvertToDTO(ComplexTour complexTour, LocationService locationService, TourRequestService tourRequestService)
        {
            if (complexTour == null)
            {
                return null;
            }

            string locations = "";
            string languages = "";
            string startDates = "";
            string endDates = "";
            string statuses = "";

            foreach (int requestId in complexTour.AvailableTourRequestIds)
            {
                TourRequest tourRequest = tourRequestService.GetById(requestId);
                Location location = locationService.GetById(tourRequest.LocationId);

                locations += location.City + ", " + location.Country + "\n";
                if (!string.IsNullOrEmpty(locations))
                    locations = locations.TrimEnd('\n');

                languages += tourRequest.Language + "\n";
                if (!string.IsNullOrEmpty(languages))
                    languages = languages.TrimEnd('\n');

                startDates += tourRequest.StartTime.ToString() + "\n";
                if (!string.IsNullOrEmpty(startDates))
                    startDates = startDates.TrimEnd('\n');

                endDates += tourRequest.EndTime.ToString() + "\n";
                if (!string.IsNullOrEmpty(endDates))
                    endDates = endDates.TrimEnd('\n');

                statuses += tourRequest.Status + "\n";
                if (!string.IsNullOrEmpty(statuses))
                    statuses = statuses.TrimEnd('\n');
            }

            return new ComplexTourRequestDTO(complexTour.Id, locations, languages, startDates, endDates, statuses);
        }
        
        static public List<ComplexTourRequestDTO> ConvertToDTOList(List<ComplexTour> complexTours, LocationService locationService, TourRequestService tourRequestService)
        {
            List<ComplexTourRequestDTO> dtos = new List<ComplexTourRequestDTO>();

            foreach (ComplexTour complexTour in complexTours)
            {
                dtos.Add(ConvertToDTO(complexTour, locationService, tourRequestService));
            }

            return dtos;
        }
    }

}
