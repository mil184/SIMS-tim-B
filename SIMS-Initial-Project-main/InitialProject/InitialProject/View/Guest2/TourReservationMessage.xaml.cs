using InitialProject.Model;
using InitialProject.Service;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace InitialProject.View.Guest2
{
    /// <summary>
    /// Interaction logic for TourReservationMessage.xaml
    /// </summary>
    public partial class TourReservationMessage : Window
    {
        TourReservation TourReservation;
        User LoggedInUser;

        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        private readonly UserService _userService;
        public TourReservationMessage(TourReservation tourReservation, User user, TourService tourService, LocationService locationService, UserService userService)
        {
            InitializeComponent();
            DataContext = this;

            TourReservation = tourReservation;
            LoggedInUser = user;

            _tourService = tourService;
            _locationService = locationService;
            _userService = userService;
        }

        private void PrintReportButton_Click(object sender, RoutedEventArgs e)
        {
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
                XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold, options);
                XFont subtitleFont = new XFont("Arial", 12, XFontStyle.Bold, options);
                XFont contentFont = new XFont("Arial", 10, XFontStyle.Regular, options);

                XFont titleFont1 = new XFont("Arial", 16, XFontStyle.Bold);
                XSolidBrush titleBackgroundBrush = new XSolidBrush(XColor.FromArgb(237, 171, 90));

                XRect pageBounds = new XRect(40, 40, page.Width - 80, page.Height - 80);
                XRect titleBounds = new XRect(pageBounds.Left, pageBounds.Top, pageBounds.Width, 30);
                XRect contentBounds = new XRect(pageBounds.Left, titleBounds.Bottom + 10, pageBounds.Width, pageBounds.Height - 40);

                gfx.DrawRectangle(titleBackgroundBrush, titleBounds);
                gfx.DrawString("Reservation", titleFont1, XBrushes.Black, titleBounds, XStringFormats.Center);

                XPoint lineStart = new XPoint(titleBounds.Left, titleBounds.Bottom + 2);
                XPoint lineEnd = new XPoint(titleBounds.Right, titleBounds.Bottom + 2);
                gfx.DrawLine(XPens.Black, lineStart, lineEnd);

                XRect userDetailsBounds = new XRect(contentBounds.Left, contentBounds.Top, 150, 20);
                gfx.DrawString("User details", subtitleFont, XBrushes.Black, userDetailsBounds, XStringFormats.TopLeft);

                string firstName = LoggedInUser.Username;
               // string id = TourReservation.UserId.ToString();
                Tour tour = _tourService.GetById(TourReservation.TourId);
                Location location = _locationService.GetById(tour.LocationId);
                User guide = _userService.GetById(tour.GuideId);
                string tourName = tour.Name;
                string tourLocation = location.City + ", " + location.Country;
                string date = TourReservation.ReservationTime.ToString();
                string tourdate = tour.StartTime.ToString();
                string guests = TourReservation.PersonCount.ToString();
                string tourGuide = guide.Username;

                XRect userDetailsTextBounds = new XRect(userDetailsBounds.Left, userDetailsBounds.Bottom + 2, 200, 12);
                gfx.DrawString("First Name:", contentFont, XBrushes.Black, userDetailsTextBounds, XStringFormats.TopLeft);
                gfx.DrawString(firstName, contentFont, XBrushes.Black, new XRect(userDetailsTextBounds.Left + 100, userDetailsTextBounds.Top, 200, 12), XStringFormats.TopLeft);

                gfx.DrawString("Reservation Date:", contentFont, XBrushes.Black, new XRect(userDetailsTextBounds.Left, userDetailsTextBounds.Bottom + 5, 200, 12), XStringFormats.TopLeft);
                gfx.DrawString(date, contentFont, XBrushes.Black, new XRect(userDetailsTextBounds.Left + 100, userDetailsTextBounds.Bottom + 5, 200, 12), XStringFormats.TopLeft);

                //gfx.DrawString("ID:", contentFont, XBrushes.Black, new XRect(userDetailsTextBounds.Left, userDetailsTextBounds.Bottom + 5, 200, 12), XStringFormats.TopLeft);
                //gfx.DrawString(id, contentFont, XBrushes.Black, new XRect(userDetailsTextBounds.Left + 100, userDetailsTextBounds.Bottom + 5, 200, 12), XStringFormats.TopLeft);

                int paragraphSpace = 20;
                XRect tourDetailsBounds = new XRect(contentBounds.Left, userDetailsTextBounds.Bottom + paragraphSpace, 150, 20);
                gfx.DrawString("Tour details", subtitleFont, XBrushes.Black, tourDetailsBounds, XStringFormats.TopLeft);

                XRect tourDetailsTextBounds = new XRect(tourDetailsBounds.Left, tourDetailsBounds.Bottom + 2, 200, 12);
                gfx.DrawString("Tour Name:", contentFont, XBrushes.Black, tourDetailsTextBounds, XStringFormats.TopLeft);
                gfx.DrawString(tourName, contentFont, XBrushes.Black, new XRect(tourDetailsTextBounds.Left + 100, tourDetailsTextBounds.Top, 200, 12), XStringFormats.TopLeft);

                gfx.DrawString("Location:", contentFont, XBrushes.Black, new XRect(tourDetailsTextBounds.Left, tourDetailsTextBounds.Bottom + 5, 200, 12), XStringFormats.TopLeft);
                gfx.DrawString(tourLocation, contentFont, XBrushes.Black, new XRect(tourDetailsTextBounds.Left + 100, tourDetailsTextBounds.Bottom + 5, 200, 12), XStringFormats.TopLeft);

                gfx.DrawString("Tour Date:", contentFont, XBrushes.Black, new XRect(tourDetailsTextBounds.Left, tourDetailsTextBounds.Bottom + 20, 200, 12), XStringFormats.TopLeft);
                gfx.DrawString(tourdate, contentFont, XBrushes.Black, new XRect(tourDetailsTextBounds.Left + 100, tourDetailsTextBounds.Bottom + 20, 200, 12), XStringFormats.TopLeft);

                gfx.DrawString("Guests:", contentFont, XBrushes.Black, new XRect(tourDetailsTextBounds.Left, tourDetailsTextBounds.Bottom + 35, 200, 12), XStringFormats.TopLeft);
                gfx.DrawString(guests, contentFont, XBrushes.Black, new XRect(tourDetailsTextBounds.Left + 100, tourDetailsTextBounds.Bottom + 35, 200, 12), XStringFormats.TopLeft);

                gfx.DrawString("Tour Guide:", contentFont, XBrushes.Black, new XRect(tourDetailsTextBounds.Left, tourDetailsTextBounds.Bottom + 50, 200, 12), XStringFormats.TopLeft);
                gfx.DrawString(tourGuide, contentFont, XBrushes.Black, new XRect(tourDetailsTextBounds.Left + 100, tourDetailsTextBounds.Bottom + 50, 200, 12), XStringFormats.TopLeft);

                int headerXPos = (int)contentBounds.Left;
                int headerYPos = (int)tourDetailsTextBounds.Bottom + paragraphSpace;
                int columnWidth = (int)contentBounds.Width / 4;

                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\reservation_report.pdf";
                document.Save(filePath);

                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
