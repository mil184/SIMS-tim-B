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
            string chosenDates = "";

            foreach (int requestId in complexTour.TourRequestIds)
            {
                TourRequest tourRequest = tourRequestService.GetById(requestId);
                Location location = locationService.GetById(tourRequest.LocationId);

                locations += location.City + ", " + location.Country + "\n";

                languages += tourRequest.Language + "\n";

                startDates += tourRequest.StartTime.ToString() + "\n";
                
                endDates += tourRequest.EndTime.ToString() + "\n";
               
                statuses += tourRequest.Status + "\n";
                
                if (tourRequest.Status == Resources.Enums.RequestStatus.accepted)
                {
                    chosenDates += tourRequest.ChosenDate.ToString() + "\n";
                }
                else
                {
                    chosenDates += "/ \n";
                }
            }

            locations = locations.TrimEnd('\n');
            languages = languages.TrimEnd('\n');
            startDates = startDates.TrimEnd('\n');
            endDates = endDates.TrimEnd('\n');
            statuses = statuses.TrimEnd('\n');
            chosenDates = chosenDates.TrimEnd('\n');

            if (!string.IsNullOrEmpty(locations))
                locations = locations.TrimEnd('\n');
            if (!string.IsNullOrEmpty(languages))
                languages = languages.TrimEnd('\n');
            if (!string.IsNullOrEmpty(startDates))
                startDates = startDates.TrimEnd('\n');
            if (!string.IsNullOrEmpty(endDates))
                endDates = endDates.TrimEnd('\n');
            if (!string.IsNullOrEmpty(statuses))
                statuses = statuses.TrimEnd('\n');
            if (!string.IsNullOrEmpty(chosenDates))
                statuses = statuses.TrimEnd('\n');

            return new ComplexTourRequestDTO(complexTour.Id, locations, languages, startDates, endDates, statuses, chosenDates, complexTour.Status.ToString());
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
