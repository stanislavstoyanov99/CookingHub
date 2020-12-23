namespace CookingHub.Models.ViewModels.CookingHubUsers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    using static CookingHub.Models.Common.ModelValidation.CookingHubUserValidation;

    public class CookingHubUserEditViewModel
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        [Required(ErrorMessage = RoleSelectedError)]
        public string NewRole { get; set; }

        public List<SelectListItem> RolesList { get; set; }
    }
}
