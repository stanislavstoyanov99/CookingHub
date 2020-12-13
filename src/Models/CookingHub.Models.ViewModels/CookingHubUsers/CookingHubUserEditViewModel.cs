using System;
using System.Collections.Generic;
using System.Text;

namespace CookingHub.Models.ViewModels.CookingHubUsers
{
    public class CookingHubUserEditViewModel
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public bool IsSelected { get; set; }
    }
}
