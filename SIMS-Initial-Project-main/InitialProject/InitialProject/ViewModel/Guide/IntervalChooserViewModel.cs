using InitialProject.Model.DTO;
using InitialProject.Resources.Observer;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using iTextSharp.text.pdf.parser.clipper;
using Path = System.IO.Path;
using InitialProject.Service;
using InitialProject.Converters;
using InitialProject.Model;
using InitialProject.ViewModel.Guide;
using MenuNavigation.Commands;
using InitialProject.Commands;
using InitialProject.View.Guest2;
using System.Windows.Interop;

namespace InitialProject.ViewModel.Guide
{
    public class IntervalChooserViewModel : INotifyPropertyChanged
    {
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        public ObservableCollection<GuideTourDTO> UpcomingTours { get; set; }

        public User CurrentUser { get; set; }

        private DateTime? _startDateInput;
        public DateTime? StartDateInput
        {
            get => _startDateInput;
            set
            {
                if (value != _startDateInput)
                {
                    _startDateInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime? _endDateInput;
        public DateTime? EndDateInput
        {
            get => _endDateInput;
            set
            {
                if (value != _endDateInput)
                {
                    _endDateInput = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _leftDatePickerError;
        public string LeftDatePickerError
        {
            get => _leftDatePickerError;
            set
            {
                if (value != _leftDatePickerError)
                {
                    _leftDatePickerError = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _rightDatePickerError;
        public string RightDatePickerError
        {
            get => _rightDatePickerError;
            set
            {
                if (value != _rightDatePickerError)
                {
                    _rightDatePickerError = value;
                    OnPropertyChanged();
                }
            }
        }
        public IntervalChooserViewModel(TourService tourService, LocationService locationService, User user) 
        {
            _tourService = tourService;
            _locationService = locationService;
            CurrentUser = user;


            UpcomingTours = new ObservableCollection<GuideTourDTO>();

            foreach (GuideTourDTO tour in GuideDTOConverter.ConvertToDTO(_tourService.GetUpcomingTours(CurrentUser), _locationService))
            {
                UpcomingTours.Add(tour);
            }
            InitializeRelayCommands();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void PrintReceipt(ObservableCollection<GuideTourDTO> upcomingTours, DateTime leftBoundary, DateTime rightBoundary)
        {
            try
            {

                PdfPTable pdfTable = new PdfPTable(3);
                pdfTable.WidthPercentage = 80;
                pdfTable.DefaultCell.BorderWidth = 1.0f;

                string logoPath = "../../../Resources/Images/logo.png";
                iTextSharp.text.Image logoImage = iTextSharp.text.Image.GetInstance(logoPath);
                logoImage.ScaleAbsolute(20f, 20f); // Adjust the size as per your requirements

                // Add Company Name and Logo
                PdfPCell companyCell = new PdfPCell(new Phrase("DMJM-Tours", FontFactory.GetFont("Times New Roman", 18, Font.BOLD, new BaseColor(64, 134, 170))));
                companyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                companyCell.BorderWidth = 0;
                companyCell.Colspan = 3;
                companyCell.PaddingBottom = 10f;
                pdfTable.AddCell(companyCell);

                // Add Upcoming Tours Header
                PdfPCell headerCell = new PdfPCell(new Phrase("Upcoming Tours", FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.WHITE)));
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell.BackgroundColor = new BaseColor(64, 134, 170);
                headerCell.BorderWidth = 0;
                headerCell.Colspan = 3;
                headerCell.Padding = 8f;
                pdfTable.AddCell(headerCell);

                // Add Date Range   

                string dateRange = leftBoundary.ToString("d.M.yyyy") + " - " + rightBoundary.ToString("d.M.yyyy");
                PdfPCell dateRangeCell = new PdfPCell(new Phrase(dateRange, FontFactory.GetFont("Times New Roman", 12, Font.NORMAL, BaseColor.WHITE)));
                dateRangeCell.HorizontalAlignment = Element.ALIGN_CENTER;
                dateRangeCell.BackgroundColor = new BaseColor(64, 134, 170);
                dateRangeCell.BorderWidth = 0;
                dateRangeCell.Colspan = 3;
                dateRangeCell.Padding = 8f;
                pdfTable.AddCell(dateRangeCell);


                // Table Header
                PdfPCell cellHeader1 = new PdfPCell(new Phrase("Tour Name", FontFactory.GetFont("Times New Roman", 12, Font.BOLD)));
                cellHeader1.BackgroundColor = new BaseColor(220, 220, 220);
                cellHeader1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cellHeader1);

                PdfPCell cellHeader2 = new PdfPCell(new Phrase("Location", FontFactory.GetFont("Times New Roman", 12, Font.BOLD)));
                cellHeader2.BackgroundColor = new BaseColor(220, 220, 220);
                cellHeader2.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cellHeader2);

                PdfPCell cellHeader3 = new PdfPCell(new Phrase("Start Time", FontFactory.GetFont("Times New Roman", 12, Font.BOLD)));
                cellHeader3.BackgroundColor = new BaseColor(220, 220, 220);
                cellHeader3.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cellHeader3);

                // Table Data
                foreach (GuideTourDTO tour in upcomingTours)
                {
                    pdfTable.AddCell(new Phrase(tour.Name, FontFactory.GetFont("Times New Roman", 11)));
                    pdfTable.AddCell(new Phrase(tour.Location, FontFactory.GetFont("Times New Roman", 11)));
                    pdfTable.AddCell(new Phrase(tour.StartTime.ToString(), FontFactory.GetFont("Times New Roman", 11)));
                }

                #region Pdf Generation
                string folderPath = "../../../Resources/Reports/Guide";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // File Name
                int fileCount = Directory.GetFiles(folderPath).Length;
                string strFileName = "UpcomingGuideTours" + (fileCount + 1) + ".pdf";
                string filePath = Path.Combine(folderPath, strFileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }
                #endregion

                #region Display PDF
                // Specify the default program to open the PDF file
                ProcessStartInfo psi = new ProcessStartInfo(filePath);
                psi.UseShellExecute = true;
                psi.Verb = "open";

                Process.Start(psi);
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DateSelectionChanged() 
        {
            if (StartDateInput == null || EndDateInput == null)
                return;

            UpcomingTours.Clear();

            foreach (GuideTourDTO tour in GuideDTOConverter.ConvertToDTO(_tourService.GetToursByTimeInterval(CurrentUser, StartDateInput.Value, EndDateInput.Value), _locationService))
            {
                UpcomingTours.Add(tour);
            }
        }
        private SolidColorBrush _leftDateBorderColor;
        public SolidColorBrush LeftDateBorderColor
        {
            get => _leftDateBorderColor;
            set
            {
                if (_leftDateBorderColor != value)
                {
                    _leftDateBorderColor = value;
                    OnPropertyChanged(nameof(LeftDateBorderColor));
                }
            }
        }
        private SolidColorBrush _rightDateBorderColor;
        public SolidColorBrush RightDateBorderColor
        {
            get => _rightDateBorderColor;
            set
            {
                if (_rightDateBorderColor != value)
                {
                    _rightDateBorderColor = value;
                    OnPropertyChanged(nameof(RightDateBorderColor));
                }
            }
        }
        public void Report() 
        {
            LeftDatePickerError = "";
            RightDatePickerError = "";
            LeftDateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#007ACC"));
            RightDateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#007ACC"));

            bool valid = true;
            if(StartDateInput == null) 
            {
                LeftDatePickerError = "Please enter a start date.";
                LeftDateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                valid = false;
            }
            if (EndDateInput == null)
            {
                RightDatePickerError = "Please enter an end date.";
                RightDateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                valid = false;

            }

            if (StartDateInput < DateTime.Today)
            {
                LeftDatePickerError = "Please enter a date before today.";
                LeftDateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                valid = false;
            }
            if (EndDateInput < DateTime.Today)
            {
                RightDatePickerError = "Please enter a date before today.";
                RightDateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                valid = false;
            }
            if (StartDateInput > EndDateInput)
            {
                RightDatePickerError = "The end date should be after the start date.";
                RightDateBorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA500"));
                valid = false;
            }

            if(valid)
            PrintReceipt(UpcomingTours, StartDateInput.Value, EndDateInput.Value);
        }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand GenerateReportCommand { get; private set; }

        private void InitializeRelayCommands()
        {
            GenerateReportCommand = new RelayCommand(GenerateReport);
        }
        public void GenerateReport(object parameter)
        {
            Report();
        }
    }
}
