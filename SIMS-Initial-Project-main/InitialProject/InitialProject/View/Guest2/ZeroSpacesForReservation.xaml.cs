using InitialProject.Model;
using InitialProject.Model.DTO;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using InitialProject.Service;
using InitialProject.ViewModel.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.View.Guest2
{
    public partial class ZeroSpacesForReservation : Window
    {
        private readonly ZeroSpacesForReservationViewModel _viewModel;

        public ZeroSpacesForReservation(ZeroSpacesForReservationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            if (_viewModel.CloseAction == null)
            {
                _viewModel.CloseAction = new Action(this.Close);
            }
        }

    }
}
