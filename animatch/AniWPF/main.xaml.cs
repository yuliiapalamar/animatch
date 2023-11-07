﻿using System;
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
using AniDAL.Repositories;
using AniWPF.StartupHelper;
using System.Windows.Controls;
using System.ComponentModel;
using AniBLL.Services;

namespace AniWPF
{
    
    public partial class main : Window
    {
        private readonly IAnimeService _animeService;
        private AnimeViewModel _viewModel;
        private readonly IAbstractFactory<random> _random_factory;


        public main(IAnimeService animeService, IAbstractFactory<random> rfactory)
        {
            InitializeComponent();
            _animeService = animeService;
            _random_factory = rfactory;
            
            
        // Створюємо екземпляр ViewModel і встановлюємо його як DataContext
            _viewModel = new AnimeViewModel(_animeService,1);
            DataContext = _viewModel;
        }
        public class AnimeViewModel : INotifyPropertyChanged
        {
            private readonly IAnimeService _animeService;
            private int id;

            public AnimeViewModel(IAnimeService animeService, int id)
            {
                _animeService = animeService;
                this.id = id;
            }
            public string AnimeName
            {
                get { return _animeService.GetById(id).Name; }
            }

            public string AnimeText
            {
                get { return _animeService.GetById(id).Text; }

            }
            public double AnimeRate
            {
                get
                {
                    return _animeService.GetById(id).Imdbrate;
                }
                set
                {
                    // Встановлюємо значення rate в джерелі даних або де зручно.
                    OnPropertyChanged(nameof(AnimeRate)); // Сповіщаємо систему про зміну значення
                }
            }
            public string AnimePhoto
            {
                get
                {
                    return _animeService.GetById(id).Photo;
                }
            }

            public int AnimeYear
            {
                get
                {
                    return _animeService.GetById(id).Year;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void random_Click(object sender, RoutedEventArgs e)
        {
            _random_factory.Create().Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _random_factory.Create().Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _random_factory.Create().Show();
        }
    }
    
}
