﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using AniBLL.Services;

namespace AniWPF
{
    public partial class RandomWindow : Window
    {
        private readonly IAnimeService animeService;
        private readonly IAnimeGenreService animeGenreService;
        private int id;

        private AnimeViewModel viewModel;

        public RandomWindow(IAnimeService animeService, IAnimeGenreService animeGenreService)
        {
            this.animeService = animeService;
            this.animeGenreService = animeGenreService;
            this.id = LogInWindow.CurrentUserID;
            this.InitializeComponent();
            System.Random randomForAnime = new System.Random();

            // Створюємо екземпляр ViewModel і встановлюємо його як DataContext
            this.viewModel = new AnimeViewModel(this.animeService, this.animeGenreService, randomForAnime.Next(1, 50));
            this.DataContext = this.viewModel;

            this.WindowState = WindowState.Maximized;
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
                        result += item;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Random randomForAnime = new System.Random();
            this.viewModel = new AnimeViewModel(this.animeService, this.animeGenreService, randomForAnime.Next(1, 50));
        }

        private void Random_Click(object sender, RoutedEventArgs e)
        {
            System.Random randomForAnime = new System.Random();
            this.viewModel = new AnimeViewModel(this.animeService, this.animeGenreService, randomForAnime.Next(1, 50));
            this.DataContext = this.viewModel;
        }
    }
}
