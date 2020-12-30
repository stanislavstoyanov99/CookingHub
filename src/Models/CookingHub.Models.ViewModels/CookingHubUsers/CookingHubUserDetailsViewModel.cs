namespace CookingHub.Models.ViewModels.CookingHubUsers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Services.Mapping;
    using CookingHub.Data.Models;
    using CookingHub.Data.Models.Enumerations;

    using static CookingHub.Models.Common.ModelValidation;
    using static CookingHub.Models.Common.ModelValidation.CookingHubUserValidation;

    public class CookingHubUserDetailsViewModel : IMapFrom<CookingHubUser>
    {
        [Display(Name = IdDisplayName)]
        public string Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        [Display(Name = CreatedOnDisplayName)]
        public DateTime CreatedOn { get; set; }

        public bool isDeleted { get; set; }

        public Gender Gender { get; set; }

        public IEnumerable<ApplicationRole> UserRoles { get; set; }
    }
}
