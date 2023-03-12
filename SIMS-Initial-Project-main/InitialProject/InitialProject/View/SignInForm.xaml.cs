﻿using InitialProject.Forms;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View.Guide;
using InitialProject.View.Guest2;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if(user.Password == txtPassword.Password)
                {
                    if (user.Type == InitialProject.Resources.Enums.UserType.example)
                    {
                        CommentsOverview commentsOverview = new CommentsOverview(user);
                        commentsOverview.Show();
                        Close();
                    }
                    else if (user.Type == InitialProject.Resources.Enums.UserType.owner) 
                    { 

                    }
                    else if (user.Type == InitialProject.Resources.Enums.UserType.guest1)
                    {

                    }
                    else if (user.Type == InitialProject.Resources.Enums.UserType.guide)
                    {
                        GuideWindow guideWindow = new GuideWindow(user);
                        guideWindow.Show();
                        Close();
                    }
                    else if (user.Type == InitialProject.Resources.Enums.UserType.guest2)
                    {
                        Guest2Window guest2Window = new Guest2Window(user);
                        guest2Window.Show();
                        Close();
                    }
                } 
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
    }
}
