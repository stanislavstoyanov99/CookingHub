namespace CookingHub.Models.Common
{
    public static class ModelValidation
    {
        public const string NameLengthError = "Name must be between {2} and {1} symbols";
        public const string EmptyFieldLengthError = "Please enter the field.";
        public const string IdDisplayName = "No.";

        public static class PrivacyValidation
        {
            public const int PageContentMinLength = 1000;
            public const int PageContentMaxLength = 15000;

            public const string PageContentLengthError = "Page content must be between {2} and {1} symbols";
            public const string PageContentDisplayName = "Page Content";
        }

        public static class FaqEntryValidation
        {
            public const int QuestionMinLength = 10;
            public const int QuestionMaxLength = 100;

            public const int AnswerMinLength = 10;
            public const int AnswerMaxLength = 1000;

            public const string QuestionLengthError = "Question must be between {2} and {1} symbols";
            public const string AnswerLengthError = "Answer must be between {2} and {1} symbols";
        }

        public static class CategoryValidation
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 30;
            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 500;
            public const string DescriptionError = "Description must be between {2} and {1} symbols";
            public const string CategoryIdError = "Please select category name.";
        }
        public static class RecipeValidation
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 20000;
            public const string DescriptionError = "Description must be between {2} and {1} symbols";
            public const string RecepieIdError = "Please select category name.";
            public const int IngredientsMaxLength = 20000;
            public const int ImagePathMaxLength = 1000;
        }

        public static class ArticleValidation
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 30;
            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 20000;
            public const string DescriptionError = "Description must be between {2} and {1} symbols";
            public const string ArticleIdError = "Please select category name.";
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

            public const string FirstNameLengthError = "First name must be between {2} and {1} symbols";
            public const string LastNameLengthError = "Last name must be between {2} and {1} symbols";
            public const string SubjectLengthError = "Subject must be between {2} and {1} symbols";
            public const string ContentLengthError = "Content must be between {2} and {1} symbols";

            public const string FirstNameDisplayName = "First Name";
            public const string LastNameDispalyName = "Last Name";
        }
    }
}
