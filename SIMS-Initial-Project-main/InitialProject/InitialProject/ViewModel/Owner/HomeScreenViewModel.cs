using InitialProject.Model;
using InitialProject.Service;
using InitialProject.View.Owner;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MenuNavigation.Commands;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace InitialProject.ViewModel.Owner
{
    public class HomeScreenViewModel
    {
        public OwnerMainWindow MainWindow { get; set; }
        public NavigationService navigationService { get; set; }
        public User LoggedInUser { get; set; }

        private readonly AccommodationService _accommodationService;
        private readonly AccommodationRatingService _ratingService;

        public RelayCommand NavigateToAccommodationsPageCommand { get; set; }
        public RelayCommand NavigateToRenovationsPageCommand { get; set; }
        public RelayCommand NavigateToGuestReviewPageCommand { get; set; }
        public RelayCommand NavigateToRatingsPageCommand { get; set; }
        public RelayCommand NavigateToRescheduleRequestsPageCommand { get; set; }
        public RelayCommand NavigateToForumsPageCommand { get; set; }
        public RelayCommand GenerateReportCommand { get; set; }
        public RelayCommand PlayMusicCommand { get; set; }

        private void Execute_NavigateToAccommodationsPageCommand(object obj)
        {
            Page accommodations = new AccommodationsPage(navigationService, MainWindow, LoggedInUser);
            navigationService.Navigate(accommodations);
        }

        private void Execute_NavigateToRenovationsPageCommand(object obj)
        {
            Page renovations = new RenovationsPage(LoggedInUser);
            navigationService.Navigate(renovations);
        }

        private void Execute_NavigateToGuestReviewPageCommand(object obj)
        {
            Page reviews = new GuestReviewPage(LoggedInUser);
            navigationService.Navigate(reviews);
        }

        private void Execute_NavigateToRatingsPageCommand(object obj)
        {
            Page ratings = new RatingsPage(LoggedInUser);
            navigationService.Navigate(ratings);
        }

        private void Execute_NavigateToRescheduleRequestsPageCommand(object obj)
        {
            Page reschedules = new RescheduleRequestsPage(navigationService, LoggedInUser);
            navigationService.Navigate(reschedules);
        }

        private void Execute_NavigateToForumsPageCommand(object obj)
        {
            Page forums = new ForumsPage(navigationService, LoggedInUser);
            navigationService.Navigate(forums);
        }

        private void Execute_GenerateReportCommand(object obj)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("output.pdf", FileMode.Create));
            document.Open();

            PdfPTable head = new PdfPTable(2);
            head.WidthPercentage = 100;
            PdfPCell nameCell = new PdfPCell(new Phrase("Cveta, Owner"));
            nameCell.HorizontalAlignment = Element.ALIGN_LEFT;
            nameCell.Border = PdfPCell.NO_BORDER;
            PdfPCell dateCell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd-MM-yyyy") + ", DMJM Tours"));
            dateCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            dateCell.Border = PdfPCell.NO_BORDER;
            head.AddCell(nameCell);
            head.AddCell(dateCell);
            document.Add(head);

            Paragraph title = new Paragraph("\nAverage Accommodation Ratings\n", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD));
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            document.Add(title);

            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;

            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            PdfPCell headerCell1 = new PdfPCell(new Phrase("Accommodation Name", headerFont));
            PdfPCell headerCell2 = new PdfPCell(new Phrase("Cleanliness", headerFont));
            PdfPCell headerCell3 = new PdfPCell(new Phrase("Correctness", headerFont));
            PdfPCell headerCell4 = new PdfPCell(new Phrase("Average", headerFont));
            headerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell1.Border = PdfPCell.NO_BORDER;
            headerCell2.Border = PdfPCell.NO_BORDER;
            headerCell3.Border = PdfPCell.NO_BORDER;
            headerCell4.Border = PdfPCell.NO_BORDER;
            table.AddCell(headerCell1);
            table.AddCell(headerCell2);
            table.AddCell(headerCell3);
            table.AddCell(headerCell4);

            foreach (var accommodation in _accommodationService.GetByUser(LoggedInUser.Id))
            {
                double cleanliness = _ratingService.GetAverageRatings(accommodation.Id, "Cleanliness");
                double corectness = _ratingService.GetAverageRatings(accommodation.Id, "Correctness");
                PdfPCell cell1 = new PdfPCell(new Phrase(accommodation.Name));
                PdfPCell cell2 = new PdfPCell(new Phrase(cleanliness.ToString()));
                PdfPCell cell3 = new PdfPCell(new Phrase(corectness.ToString()));
                PdfPCell cell4 = new PdfPCell(new Phrase(((corectness + cleanliness) / 2).ToString()));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell1);
                table.AddCell(cell2);
                table.AddCell(cell3);
                table.AddCell(cell4);
            }

            document.Add(table);


            double Cleanliness = _ratingService.GetAverageRatings("Cleanliness", LoggedInUser.Id);
            double Corectness = _ratingService.GetAverageRatings("Correctness", LoggedInUser.Id);
            Paragraph exces = new Paragraph("\nTotal average cleanliness grade is " + _ratingService.GetAverageRatings("Cleanliness", LoggedInUser.Id)
                + " and total average correctness grade is " + +_ratingService.GetAverageRatings("Correctness", LoggedInUser.Id) + ".\n"
                + "Total average grade is " + ((Cleanliness + Corectness) / 2).ToString() + ".\n");
            document.Add(exces);

            document.Close();

            string filePath = "output.pdf";
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }

        private void Execute_PlayMusicCommand(object obj)
        {
            if (!MainWindow._viewModel.IsPlaying)
            {
                MainWindow._viewModel.Player.Play();
                MainWindow._viewModel.IsPlaying = true;
            }
            else
            {
                MainWindow._viewModel.Player.Stop();
                MainWindow._viewModel.IsPlaying = false;
            }
        }

        private bool CanExecute_Command(object obj)
        {
            return true;
        }

        public HomeScreenViewModel(OwnerMainWindow window)
        {
            _accommodationService = new AccommodationService();
            _ratingService = new AccommodationRatingService();

            NavigateToAccommodationsPageCommand = new RelayCommand(Execute_NavigateToAccommodationsPageCommand, CanExecute_Command);
            NavigateToRenovationsPageCommand = new RelayCommand(Execute_NavigateToRenovationsPageCommand, CanExecute_Command);
            NavigateToGuestReviewPageCommand = new RelayCommand(Execute_NavigateToGuestReviewPageCommand, CanExecute_Command);
            NavigateToRatingsPageCommand = new RelayCommand(Execute_NavigateToRatingsPageCommand, CanExecute_Command);
            NavigateToRescheduleRequestsPageCommand = new RelayCommand(Execute_NavigateToRescheduleRequestsPageCommand, CanExecute_Command);
            NavigateToForumsPageCommand = new RelayCommand(Execute_NavigateToForumsPageCommand, CanExecute_Command);
            GenerateReportCommand = new RelayCommand(Execute_GenerateReportCommand, CanExecute_Command);
            PlayMusicCommand = new RelayCommand(Execute_PlayMusicCommand, CanExecute_Command);

            MainWindow = window;
            navigationService = window.navigationService;
            LoggedInUser = window.LoggedInUser;
        }
    }
}
