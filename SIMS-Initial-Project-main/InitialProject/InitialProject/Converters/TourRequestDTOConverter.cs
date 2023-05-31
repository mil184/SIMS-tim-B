using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Service;
using System.Collections.Generic;

namespace InitialProject.Converters
{
    public class TourRequestDTOConverter
    {
        private readonly LocationService _locationService = new LocationService();

        public TourRequestDTOConverter(LocationService locationService)
        {
            _locationService = locationService;
        }

        public List<Guest2TourRequestDTO> ConvertToDTOList(List<TourRequest> tourRequests)
        {
            List<Guest2TourRequestDTO> DTOList = new List<Guest2TourRequestDTO>();

            foreach (TourRequest tourRequest in tourRequests)
            {
                DTOList.Add(ConvertToDTO(tourRequest));
            }

            return DTOList;
        }

        public Guest2TourRequestDTO ConvertToDTO(TourRequest tourRequest)
        {
            return new Guest2TourRequestDTO(
                    tourRequest.Id,
                    _locationService.GetById(tourRequest.LocationId).Country,
                    _locationService.GetById(tourRequest.LocationId).City,
                    tourRequest.Description,
                    tourRequest.Language,
                    tourRequest.MaxGuests,
                    tourRequest.StartTime,
                    tourRequest.EndTime,
                    tourRequest.Status);
        }
    }
}
