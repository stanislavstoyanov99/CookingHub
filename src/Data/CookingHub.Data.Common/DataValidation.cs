namespace CookingHub.Data.Common
{
    public static class DataValidation
    {
        public const int FullNameMaxLength = 60;
        public const int NameMaxLength = 30;

        public static class PrivacyValidation
        {
            public const int ContentPageMaxLength = 15000;
        }

        public static class FaqEntryValidation
        {
            public const int QuestionMaxLength = 100;
            public const int AnswerMaxLength = 1000;
        }

        public static class ContactFormEntryValidation
        {
            public const int SubjectMaxLength = 100;
            public const int ContentMaxLength = 10000;
        }

        public static class CategoryValidation
        {
            public const int NameMaxLength = 30;
            public const int DescriptionMaxLength = 500;
        }

        public static class ArticleValidation
        {
            public const int TitleMaxLength = 50;
            public const int DescriptionMaxLength = 10000;
            public const int ImagePathMaxLength = 1000;
        }

        public static class ArticleCommentValidation
        {
            public const int ContentMaxLength = 300;
        }

        public static class RecipeValidation
        {
            public const int NameMaxLength = 30;
            public const int DescriptionMaxLength = 20000;
            public const int IngredientsMaxLength = 20000;
            public const int ImagePathMaxLength = 1000;
        }

        public static class ReviewValidation
        {
            public const int TitleMaxLength = 30;
            public const int DescriptionMaxLength = 1500;
        }

        public static class ReviewCommentValidation
        {
            public const int ContentMaxLength = 300;
        }

        public static class MessageValidation
        {
            public const int ContentMaxLength = 1000;
        }
    }
}
