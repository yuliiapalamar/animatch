﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using AniBLL.Models;
using AniBLL.Services;
using AniWPF.StartupHelper;
using AniWPF.ViewModels;
using Microsoft.Extensions.Logging;

namespace AniWPF
{
    
    public partial class RedactWindow : Window
    {
        private readonly ILogger<RedactWindow> logger;

        private readonly IAbstractFactory<RandomWindow> randomFactory;
        private readonly IAbstractFactory<MainWindow> mainFactory;
        private readonly IAbstractFactory<ProfileWindow> profileFactory;

        private readonly IUserService userService;
        private readonly IAddedAnimeService addedAnimeService;
        private readonly IAnimeService animeService;

        private UserViewModel viewModel;
        private int id;

        public RedactWindow(IUserService userService, IAddedAnimeService addedAnimeService, 
            IAnimeService animeService, IAbstractFactory<RandomWindow> randomFactory, 
            IAbstractFactory<MainWindow> mainFactory, IAbstractFactory<ProfileWindow> profileFactory,
            ILogger<RedactWindow> logger)
        {
            this.randomFactory = randomFactory;
            this.mainFactory = mainFactory;
            this.profileFactory = profileFactory;

            this.userService = userService;
            this.addedAnimeService = addedAnimeService;
            this.animeService = animeService;

            this.id = LogInWindow.CurrentUserID;

            this.viewModel = new UserViewModel(this.userService, this.id);
            this.DataContext = this.viewModel;
            List<AnimeModel> temp = addedAnimeService.GetAddedAnimesForUser(this.id);

            this.logger = logger;
            this.logger.LogInformation("RedactWindow created");

            this.InitializeComponent();
            this.WindowState = WindowState.Maximized;
            
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click Watched button, changes was canceled");
            this.profileFactory.Create(this).Show();
            this.Close();
        }
        
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click Watched button, changes was save");
            if(name.Text == null && description.Text == null)
            {
                this.profileFactory.Create(this).Show();
                this.Close();
            }
            userService.UpdateTitleAndText(id,name.Text, description.Text);
            this.profileFactory.Create(this).Show();
            this.Close();
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click Main button");
            this.mainFactory.Create(this).Show();
            this.Close();
        }
        private void SelectPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;

                userService.UpdatePhoto(id, selectedImagePath);
            }
        }



    }
}
