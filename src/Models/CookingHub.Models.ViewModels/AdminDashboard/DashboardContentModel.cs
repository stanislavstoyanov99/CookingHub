namespace CookingHub.Models.ViewModels.AdminDashboard
{
    using CookingHub.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class DashboardContentModel
    {
        public IEnumerable<CookingHubUser> Users { get; set; }

        public Recipe Recipes;

        public Review Reviews;

        public Article Articles;
    }
}
