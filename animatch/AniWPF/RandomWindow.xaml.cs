﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using AniBLL.Services;
using AniWPF.StartupHelper;
using Microsoft.Extensions.Logging;
using AniBLL.Models;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace AniWPF
{
    public partial class RandomWindow : Window
    {
        private readonly ILogger<RandomWindow> logger;

        public static Window? ParentWindow { get; set; }
        private readonly IAbstractFactory<ProfileWindow> profileFactory;
        private readonly IAbstractFactory<MainWindow> mainFactory;
        private readonly IAbstractFactory<LikedAnimeWindow> likedAnimeFactory;
        private readonly IAbstractFactory<SearchWindow> searchFactory;
        private readonly IAbstractFactory<AnimeWindow> animeFactory;

        private readonly IAnimeService animeService;
        private readonly IAnimeGenreService animeGenreService;
        private readonly ILikedAnimeService likedAnimeService;

        private int id;
        private AnimeViewModel viewModel;
        public static int randomAnimeId { get; set; }

        public RandomWindow(IAnimeService animeService, IAnimeGenreService animeGenreService, 
            IAbstractFactory<ProfileWindow> profileFactory, IAbstractFactory<MainWindow> mainFactory,
            ILogger<RandomWindow> logger, IAbstractFactory<LikedAnimeWindow> likedAnimeFactory,
            IAbstractFactory<SearchWindow> searchFactory, IAbstractFactory<AnimeWindow> animeFactory, ILikedAnimeService likedAnimeService)
        {
            this.InitializeComponent();
            this.WindowState = WindowState.Maximized;

            this.profileFactory = profileFactory;
            this.mainFactory = mainFactory;
            this.profileFactory = profileFactory;
            this.likedAnimeFactory = likedAnimeFactory;
            this.searchFactory = searchFactory;
            this.animeFactory = animeFactory;

            this.animeService = animeService;
            this.animeGenreService = animeGenreService;

            this.id = LogInWindow.CurrentUserID;
            System.Random randomForAnime = new System.Random();

            if (ParentWindow != null)
            {
                if (ParentWindow.GetType() == typeof(AnimeWindow))
                {
                    randomAnimeId = AnimeWindow.AnimeId;
                }
            }
            else
            {
                randomAnimeId = randomForAnime.Next(1, 51);
            }

            this.viewModel = new AnimeViewModel(this.animeService, this.animeGenreService, randomAnimeId);
            this.DataContext = this.viewModel;

            this.logger = logger;
            this.logger.LogInformation("RandomWindow створено.");
            this.likedAnimeService = likedAnimeService;
        }

        public class AnimeViewModel : INotifyPropertyChanged
        {
            private readonly IAnimeService animeService;
            private readonly IAnimeGenreService animeGenreService;

            private int id;

            public AnimeViewModel(IAnimeService animeService, IAnimeGenreService animeGenreService, int id)
            {
                this.animeService = animeService;
                this.animeGenreService = animeGenreService;
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
                get
                {
                    return this.animeService.GetById(this.id).Imdbrate;
                }

                set
                {
                    // Встановлюємо значення rate в джерелі даних або де зручно.
                    this.OnPropertyChanged(nameof(this.AnimeRate)); // Сповіщаємо систему про зміну значення
                }
            }

            public string AnimePhoto
            {
                get
                {
                    return this.animeService.GetById(this.id).Photo;
                }
            }

            public int AnimeYear
            {
                get
                {
                    return this.animeService.GetById(this.id).Year;
                }
            }

            public string AnimeGenres
            {
                get
                {
                    List<string> temp = this.animeGenreService.GetGenresForAnime(this.id);
                    string result = "";
                    foreach (string item in temp)
                    {
                        result += item + " ";
                    }
                    return result;
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private async void LikeAnime_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click LikeAnime button");
            likeUnfill.Source = new BitmapImage(new Uri("https://github.com/yuliiapalamar/animatch/blob/master/animatch/AniWPF/photo/LikedFillIcon.png?raw=true"));

            LikedAnimeModel temp = new LikedAnimeModel
            {
                Id = likedAnimeService.GetLastUserId() + 1,
                UserId = this.id,
                AnimeId = RandomWindow.randomAnimeId
            };
            likedAnimeService.Insert(temp);

            await Task.Delay(1000);
            System.Random randomForAnime = new System.Random();
            int randomAnimeId = randomForAnime.Next(1, 50);
            this.viewModel = new AnimeViewModel(this.animeService, this.animeGenreService, randomAnimeId);
            this.DataContext = this.viewModel;
            this.logger.LogInformation("New Random anime: " + animeService.GetById(randomAnimeId).Name + " was shown");
            likeUnfill.Source = new BitmapImage(new Uri("https://github.com/yuliiapalamar/animatch/blob/master/animatch/AniWPF/photo/LikedIcon.png?raw=true"));
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click Random button");
            System.Random randomForAnime = new System.Random();
            int randomAnimeId = randomForAnime.Next(1, 50);
            this.viewModel = new AnimeViewModel(this.animeService, this.animeGenreService, randomAnimeId);
            this.DataContext = this.viewModel;
            this.logger.LogInformation("New Random anime: " + animeService.GetById(randomAnimeId).Name + " was shown");
        }
        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click Profile button");
            this.profileFactory.Create(this).Show();
            this.Close();
        }

        private void ButtonAdded_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click Added button");
            this.likedAnimeFactory.Create(this).Show();
            this.Close();
        }
        private void Main_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click Main button");
            this.mainFactory.Create(this).Show();
            this.Close();
        }
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click Search button");
            this.searchFactory.Create(this).Show();
            this.Close();
        }

        private void AnimeButton_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogInformation("Click detail about anime button");
            AnimeWindow.ParentWindow = this;
            this.animeFactory.Create(this).Show();
            this.Close();
        }
    }
}
