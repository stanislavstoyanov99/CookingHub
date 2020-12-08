namespace CookingHub.Models.InputModels.Reviews
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using static CookingHub.Models.Common.ModelValidation;
    public class CreateReviewInputModel
    {
        public string Title { get; set; }

        public int RecipeId { get; set;}

        public int Rate { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string Content { get; set; }

        public string UserId { get; set; }
    }
}
