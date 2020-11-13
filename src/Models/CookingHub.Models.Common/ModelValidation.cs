namespace CookingHub.Models.Common
{
    public static class ModelValidation
    {
        public const string EmptyFieldLengthError = "Please enter the field.";

        public static class PrivacyValidation
        {
            public const int PageContentMinLength = 1000;
            public const int PageContentMaxLength = 15000;

            public const string PageContentLengthError = "Page content must be between {2} and {1} symbols";
            public const string PageContentDisplayName = "Page Content";
        }
    }
}
