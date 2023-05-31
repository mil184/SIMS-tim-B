using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DTO
{
    public class TourCreationValidation
    {
        public string TourNameValidation { get; set; }
        public string TourDescriptionValidation { get; set; }
        public string TourLanguageValidation { get; set; }
        public string TourCountryValidation { get; set; }
        public string TourCityValidation { get; set; }
        public string MaximumGuestsValidation { get; set; }
        public string DurationValidation { get; set; }
        public string DateValidation { get; set; }
        public string DateListValidation { get; set; }
        public string CheckpointListValidation { get; set; }
        public string ImageListValidation { get; set; }

        public TourCreationValidation()
        {
            TourNameValidation = string.Empty;
            TourDescriptionValidation = string.Empty;
            TourLanguageValidation = string.Empty;
            TourCountryValidation = string.Empty;
            TourCityValidation = string.Empty;
            MaximumGuestsValidation = string.Empty;
            DurationValidation = string.Empty;
            DateValidation = string.Empty;
            DateListValidation = string.Empty;
            CheckpointListValidation = string.Empty;
            ImageListValidation = string.Empty;
        }

    }
}
