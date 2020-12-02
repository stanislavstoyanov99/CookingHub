using System;
using System.Collections.Generic;
using System.Text;

namespace CookingHub.Models.ViewModels
{
   public class StatisticsResponseModel
    {
        public int[] registeredUsers { get; set; } 

        public int[] dailyUsers { get; set; }

        public int recipesCount { get; set; }

        public int articlesCount { get; set; }

        public int reviewedrecipesCount { get; set; }

        public int reviewsCount { get; set; }

        public int commentsCount { get; set; }

        public string[] topfiverecipes { get; set; }

        public string[] topfivearticles { get; set; }

    }
}
