namespace CookingHub.Models.Common
{
    public static class ModelValidation
    {
        public const string NameLengthError = "Name must be between {2} and {1} symbols.";
        public const string EmptyFieldLengthError = "Please enter the field.";
        public const string IdDisplayName = "No.";

        public static class PrivacyValidation
        {
            public const int PageContentMinLength = 1000;
            public const int PageContentMaxLength = 15000;

            public const string PageContentLengthError = "Page content must be between {2} and {1} symbols.";
            public const string PageContentDisplayName = "Page Content";
        }

        public static class FaqEntryValidation
        {
            public const int QuestionMinLength = 10;
            public const int QuestionMaxLength = 100;

            public const int AnswerMinLength = 10;
            public const int AnswerMaxLength = 1000;

            public const string QuestionLengthError = "Question must be between {2} and {1} symbols.";
            public const string AnswerLengthError = "Answer must be between {2} and {1} symbols.";
        }

        public static class CategoryValidation
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 30;

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 500;

            public const string DescriptionError = "Description must be between {2} and {1} symbols.";
            public const string CategoryIdError = "Please select category name.";
        }

        public static class RecipeValidation
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 20000;
            public const string DescriptionError = "Description must be between {2} and {1} symbols.";

            public const string RecipeIdError = "Please select category name.";

            public const int IngredientsMinLength = 10;
            public const int IngredientsMaxLength = 20000;
            public const string IngredientsError = "Ingredients must be between {2} and {1} symbols.";

            public const int ImagePathMaxLength = 1000;

            public const int PortionsMinLength = 0;
            public const int PortionsMaxLength = 12;

            public const int PreparationTimeMinLength = 0;
            public const int PreparationTimeMaxLength = 180;

            public const int CookingTimeMinLength = 0;
            public const int CookingTimeMaxLength = 180;

            public const string PreparationTimeDisplayName = "Preparation Time";
            public const string CookingTimeDisplayName = "Cooking Time";
            public const string PortionsNumberDisplayName = "Portions Number";
        }

        public static class ArticleValidation
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 30;
            public const string TitleLengthError = "Title must be between {2} and {1} symbols.";

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 20000;
            public const string DescriptionLengthError = "Description must be between {2} and {1} symbols.";

            public const int ImageMinLength = 10;
            public const int ImageMaxLength = 1000;
            public const string ImagePathError = "Image path must be between {2} and {1} symbols.";
            public const string ImageDisplayName = "Image";

            public const string ArticleIdError = "Please select article.";

            public const string CategoryDisplayName = "Category";
        }

        public static class ContactFormEntryValidation
        {
            public const int FirstNameMinLength = 3;
            public const int FirstNameMaxLength = 30;
            public const int LastNameMinLength = 3;
            public const int LastNameMaxLength = 30;

            public const int SubjectMaxLength = 100;
            public const int SubjectMinLegth = 5;

            public const int ContentMaxLength = 10000;
            public const int ContentMinLegth = 20;

            public const string FirstNameLengthError = "First name must be between {2} and {1} symbols.";
            public const string LastNameLengthError = "Last name must be between {2} and {1} symbols.";
            public const string SubjectLengthError = "Subject must be between {2} and {1} symbols.";
            public const string ContentLengthError = "Content must be between {2} and {1} symbols.";

            public const string FirstNameDisplayName = "First Name";
            public const string LastNameDispalyName = "Last Name";
        }

        public static class CookingHubUserValidation
        {
            public const string RoleSelectedError = "Please select role.";
            public const string CreatedOnDisplayName = "Created On";
        }

        public static class MessageValidation
        {
            public const int ContentMaxLength = 1000;
            public const string ContentMaxLengthError = "Content cannot be more than {0} symbols.";
            public const string UserError = "User is not logged in.";
        }
    }
}
