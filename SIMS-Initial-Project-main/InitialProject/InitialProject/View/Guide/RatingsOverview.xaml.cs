using InitialProject.Model.DTO;
using System;
using System.Collections.Generic;
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

namespace InitialProject.View.Guide
{
    /// <summary>
    /// Interaction logic for RatingsOverview.xaml
    /// </summary>
    public partial class RatingsOverview : Window
    {
        public GuideRatingDTO SelectedDTO { get; set; }
        public RatingsOverview(GuideRatingDTO guideTourDTO)
        {
            InitializeComponent();
            DataContext = this;
            SelectedDTO = guideTourDTO;

        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
