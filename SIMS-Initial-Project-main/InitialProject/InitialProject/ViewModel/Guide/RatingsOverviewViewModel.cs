using InitialProject.Model.DTO;
using iTextSharp.text.pdf;
using MenuNavigation.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InitialProject.ViewModel.Guide
{
    public class RatingsOverviewViewModel : INotifyPropertyChanged
    {
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        public GuideRatingDTO SelectedDTO { get; set; }
        public int imageIndex { get; set; }

        public List<string> Urls { get; set; }

        public RatingsOverviewViewModel(GuideRatingDTO guideRatingDTO)
        {
            SelectedDTO = guideRatingDTO;

            imageIndex = 0;
            Urls = guideRatingDTO.Urls;

            SetImage(imageIndex);
            InitializeCommands();
        }

        public void PreviousImage()
        {
            imageIndex--;
            if (imageIndex < 0)
            {
                imageIndex = Urls.Count - 1;
            }

            SetImage(imageIndex);
        }
        public void NextImage()
        {
            imageIndex++;
            if (imageIndex > Urls.Count - 1)
            {
                imageIndex = 0;
            }

            SetImage(imageIndex);
        }
        private void SetImage(int index)
        {
            try
            {
                var fullFilePath = Urls[index];

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                ImageSource = bitmap;
            }
            catch (Exception ex)
            {
                // Handle the exception by setting the Source to a default image
                BitmapImage defaultImage = new BitmapImage(new Uri("/Resources/Images/image_unavailable.png", UriKind.Relative));
                ImageSource = defaultImage;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand LeftCommand { get; private set; }
        public RelayCommand RightCommand { get; private set; }

        private void InitializeCommands()
        {
            LeftCommand = new RelayCommand(PreviousImage);
            RightCommand = new RelayCommand(NextImage);
        }
        private void NextImage(object parameter)
        {
            NextImage();
        }
        private void PreviousImage(object parameter)
        {
            PreviousImage();
        }
    }
}
