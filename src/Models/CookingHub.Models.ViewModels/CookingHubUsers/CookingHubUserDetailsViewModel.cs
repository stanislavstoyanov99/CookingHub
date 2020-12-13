namespace CookingHub.Models.ViewModels.CookingHubUsers
{
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Services.Mapping;
    using CookingHub.Data.Models;

    

    using static CookingHub.Models.Common.ModelValidation;
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using CookingHub.Data.Models.Enumerations;

    public class CookingHubUserDetailsViewModel : IMapFrom<CookingHubUser>
    {
        [Display(Name = IdDisplayName)]
        public string Id { get; set; }

        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool isDeleted { get; set; }

        public Gender Gender { get; set; }

        public IEnumerable<ApplicationRole> UserRoles { get; set; }
    }
}
