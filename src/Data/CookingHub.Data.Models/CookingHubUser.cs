namespace CookingHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookingHub.Data.Common;
    using CookingHub.Data.Common.Models;
    using CookingHub.Data.Models.Enumerations;

    using Microsoft.AspNetCore.Identity;

    public class CookingHubUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public CookingHubUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        [Required]
        [MaxLength(DataValidation.FullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
