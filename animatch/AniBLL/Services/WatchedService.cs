﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AniDAL.Repositories;
using AniDAL.DataBaseClasses;

namespace AniBLL.Services
{
    public interface IWatchedAnimeService
    {
        List<WatchedAnime> GetWatchedAnimesForUser(int userId);
        void Insert(WatchedAnime watched);
        void Delete(WatchedAnime watched);
    }
    public class WatchedService : IWatchedAnimeService
    {
        private readonly IWatchedAnimeRepository _watchedAnimeRepository; 

        public WatchedService(IWatchedAnimeRepository watchedAnimeRepository)
        {
            _watchedAnimeRepository = watchedAnimeRepository;
        }

        public List<WatchedAnime> GetWatchedAnimesForUser(int userId)
        {
            return _watchedAnimeRepository.GetWatchedAnimesForUser(userId);
        }
        public void Insert(WatchedAnime watched)
        {
            _watchedAnimeRepository.Insert(watched);
        }

        public void Delete(WatchedAnime watched)
        {
            _watchedAnimeRepository.Delete(watched);
        }
    }
}
