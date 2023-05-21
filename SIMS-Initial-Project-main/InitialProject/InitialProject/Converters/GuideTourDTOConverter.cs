using InitialProject.Model.DTO;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Converters
{
    public  class GuideTourDTOConverter
    {
        static public GuideTourDTO ConvertToDTO(Tour tour, LocationService locationService)
        {

            if (tour == null)
                return null;
            return new GuideTourDTO(
            tour.Id,
                    tour.Name,
                    locationService.GetById(tour.LocationId).Country,
                    locationService.GetById(tour.LocationId).City,
                    tour.StartTime,
                    tour.CurrentGuestCount);
        }
        static public List<GuideTourDTO> ConvertToDTO(List<Tour> tours, LocationService locationService)
        {
            List<GuideTourDTO> dtos = new List<GuideTourDTO>();
            foreach (Tour tour in tours) 
            {
                dtos.Add(ConvertToDTO(tour,locationService));
            }
            return dtos;
        }
        static public Tour ConvertToTour(GuideTourDTO dto, TourService tourService)
        {
            if (dto != null)
                return tourService.GetById(dto.Id);
            return null;
        }
    }
}
