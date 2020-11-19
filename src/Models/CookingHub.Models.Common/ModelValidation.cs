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
    }
}
