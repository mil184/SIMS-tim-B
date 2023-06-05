using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace InitialProject.View.Guest2
{
    public partial class ShowTour : Window, INotifyPropertyChanged, IObserver
    {
        public Guest2TourDTO tourDTO { get; set; }
        public int imageIndex { get; set; }

        public List<string> imageUrls { get; set; }

        private readonly TourService _tourService;
        private readonly ImageRepository _imageRepository;

        public int LanguageButtonClickCount { get; set; }
        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        public ShowTour(Guest2TourDTO tourDTO)
        {
            InitializeComponent();
            DataContext = this;

            _tourService = new TourService();
            _tourService.Subscribe(this);

            _imageRepository = new ImageRepository();
            _imageRepository.Subscribe(this);

            this.tourDTO = tourDTO;

            Tour selectedTour = ConvertToTour(tourDTO);

            imageUrls = new List<string>();

            foreach (int imageId in selectedTour.ImageIds)
            {
                if (_imageRepository.GetById(imageId) != null)
                    imageUrls.Add(_imageRepository.GetById(imageId).Url);
            }

            imageIndex = 0;
            Image image = new Image();
            var fullFilePath = imageUrls[imageIndex];
            try
            {

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                image.Source = bitmap;
                picHolder.Source = image.Source;
            }
            catch (Exception ex)
            {
                // Handle the exception by setting the Source to a default image
                BitmapImage defaultImage = new BitmapImage(new Uri("/Resources/Images/image_unavailable.png", UriKind.Relative));
                image.Source = defaultImage;

            }
            app = (App)Application.Current;
            app.ChangeLanguage(SRB);
            LanguageButtonClickCount = 0;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public Tour ConvertToTour(Guest2TourDTO dto)
        {
            return _tourService.GetById(dto.TourId);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            //
        }

        #region Images

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            imageIndex++;
            if (imageIndex > imageUrls.Count - 1)
            {
                imageIndex = 0;
            }

            var image = new Image();
            var fullFilePath = imageUrls[imageIndex];

            try
            {

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                image.Source = bitmap;
                picHolder.Source = image.Source;
            }
            catch (Exception ex)
            {
                // Handle the exception by setting the Source to a default image
                BitmapImage defaultImage = new BitmapImage(new Uri("/Resources/Images/image_unavailable.png", UriKind.Relative));
                image.Source = defaultImage;

            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            imageIndex--;

            if (imageIndex < 0)
            {
                imageIndex = imageUrls.Count - 1;
            }

            var image = new Image();
            var fullFilePath = imageUrls[imageIndex];

            try
            {

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                image.Source = bitmap;
                picHolder.Source = image.Source;
            }
            catch (Exception ex)
            {
                // Handle the exception by setting the Source to a default image
                BitmapImage defaultImage = new BitmapImage(new Uri("/Resources/Images/image_unavailable.png", UriKind.Relative));
                image.Source = defaultImage;

            }
        }

        #endregion

        public void LanguageButton_Click(object sender, RoutedEventArgs e)
        {   
            LanguageButtonClickCount++;

            if (LanguageButtonClickCount % 2 == 1)
            {
                app.ChangeLanguage(ENG);
                return;
            } 
            
            app.ChangeLanguage(SRB);
        }
    }
}
