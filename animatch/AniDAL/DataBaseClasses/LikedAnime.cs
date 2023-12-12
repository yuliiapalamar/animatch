﻿using System.ComponentModel.DataAnnotations;

namespace AniDAL.DataBaseClasses
{
    public class LikedAnime
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AnimeId { get; set; }
    }
}
