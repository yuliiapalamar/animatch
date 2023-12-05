﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using AniBLL.Services;
using AniWPF.StartupHelper;
using System.Windows.Media.Imaging;
using AniBLL.Models;
using AniWPF;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AniWPF
{
    public partial class AnimePage : Page
    {
        private readonly ILogger<AnimePage> logger;

        public Window ParentWindow { get; set; }
        private readonly IAbstractFactory<RandomWindow> randomFactory;
        private readonly IAbstractFactory<ProfileWindow> profileFactory;
        private readonly IAbstractFactory<LikedAnimeWindow> likedFactory;
        private readonly IAbstractFactory<SearchWindow> searchFactory;

        private readonly IAddedAnimeService addedAnimeService;
        private readonly ILikedAnimeService likedAnimeService;
        private readonly IDislikedAnimeService dislikedAnimeService;
        private readonly IWatchedAnimeService watchAnimeService;
        private readonly IAnimeService animeService;
        private readonly IUserService userService;
        private readonly IReviewService reviewService;



        private AnimeViewModel viewModel;
        private int id;
        private int AnimeId;

        private List<AnimeModel> uniqueAnimes;
        private List<AnimeModel> dislikedanimes;
        private List<AnimeModel> likedanimes;
        private List<AnimeModel> addedanimes;
        private List<AnimeModel> watchedanimes;

        public AnimePage(IAnimeService animeService, IAddedAnimeService addedAnimeService,
            IDislikedAnimeService dislikedAnimeService, ILikedAnimeService likedAnimeService,
            IWatchedAnimeService watchedAnimeService, IUserService userService,
            IAbstractFactory<RandomWindow> rfactory, IAbstractFactory<ProfileWindow> profileFactory,
            IAbstractFactory<LikedAnimeWindow> likedFactory, IAbstractFactory<SearchWindow> searchFactory,
            IReviewService reviewService, ILogger<AnimePage> logger)
        {
            this.InitializeComponent();
            //this.Win = WindowState.Maximized;

            this.animeService = animeService;
            this.randomFactory = rfactory;
            this.likedFactory = likedFactory;
            this.profileFactory = profileFactory;
            this.searchFactory = searchFactory;
            

            this.addedAnimeService = addedAnimeService;
            this.likedAnimeService = likedAnimeService;
            this.dislikedAnimeService = dislikedAnimeService;
            this.watchAnimeService = watchedAnimeService;
            this.userService = userService;
            this.reviewService = reviewService;

            this.id = LogInWindow.CurrentUserID;
            List<AnimeModel> animes = animeService.GetAll();
            this.dislikedanimes = dislikedAnimeService.GetDislikedAnimesForUser(id);
            this.likedanimes = likedAnimeService.GetLikedAnimesForUser(id);
            this.addedanimes = addedAnimeService.GetAddedAnimesForUser(id);
            this.watchedanimes = watchedAnimeService.GetWatchedAnimesForUser(id);

            this.uniqueAnimes = animes
                .Except(dislikedanimes)
                .Except(likedanimes)
                .Except(addedanimes)
                .Except(watchedanimes).ToList();


            this.AnimeId = MainWindow.randomAnimeId;

            this.viewModel = new AnimeViewModel(this.animeService, AnimeId, this.addedAnimeService);
            this.DataContext = this.viewModel;

            this.logger = logger;
            this.logger.LogInformation("MainWindow created");
            InitializeComponent();
        }
        public class AnimeViewModel : INotifyPropertyChanged
        {
            private readonly IAnimeService animeService;
            private readonly IAddedAnimeService addedAnime;

            private int id;

            public AnimeViewModel(IAnimeService animeService, int id, IAddedAnimeService addedAnime)
            {
                this.addedAnime = addedAnime;
                this.animeService = animeService;
                this.id = id;
            }
            public string AnimeName
            {
                get { return this.animeService.GetById(this.id).Name; }
            }

            public string AnimeText
            {
                get { return this.animeService.GetById(this.id).Text; }
            }

            public double AnimeRate
            {
                get { return Math.Round(this.animeService.GetById(this.id).Imdbrate, 2); }
            }

            public string AnimePhoto
            {
                get { return this.animeService.GetById(this.id).Photo; }
            }

            public int AnimeYear
            {
                get { return this.animeService.GetById(this.id).Year; }
            }
            public string UserLikedAnime
            {
                get { return $"{addedAnime.CountUserWhoAddAnime(this.id)} користувачів вподобали це аніме"; }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void Random_Click(object sender, RoutedEventArgs e)
        {
            //this.logger.LogInformation("Click Random button");
            //this.randomFactory.Create(this).Show();
            //this.Close();
        }
        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            //this.logger.LogInformation("Click Profile button");
            //this.profileFactory.Create(this).Show();
            //this.Close();
        }

        private void ButtonLiked_Click(object sender, RoutedEventArgs e)
        {
            //this.logger.LogInformation("Click Liked button");
            //this.likedFactory.Create(this).Show();
            //this.Close();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            //this.logger.LogInformation("Click Search button");
            //this.searchFactory.Create(this).Show();
            //this.Close();
        }
        private void LikeAnime_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click LikeAnime button");
            likeUnfill.Source = new BitmapImage(new Uri("https://github.com/yuliiapalamar/animatch/blob/master/animatch/AniWPF/photo/LikedFillIcon.png?raw=true"));

            LikedAnimeModel temp = new LikedAnimeModel
            {
                Id = likedAnimeService.GetLastUserId() + 1,
                UserId = this.id,
                AnimeId = AnimeId
            };
            likedAnimeService.Insert(temp);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            GoBackToMainWindow();
        }
        private void GoBackToMainWindow()
        {
            NavigationService?.Navigate(new Uri("MainWindow.xaml", UriKind.Relative));
        }
        private void logoButton_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFactory.Create(this).Show();
            //this.Close();
        }
    }
}
