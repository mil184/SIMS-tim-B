﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class AccommodationRatingsDTO
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string UserName { get; set; }
        public string AccommodationName { get; set; }

        public AccommodationRatingsDTO() { }

        public AccommodationRatingsDTO(int id, int reservationId, string userName, string accommodationName)
        {
            Id = id;
            ReservationId = reservationId;
            UserName = userName;
            AccommodationName = accommodationName;
        }
    }
}
