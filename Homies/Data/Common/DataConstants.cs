namespace Homies.Data.Common
{
    public static class DataConstants
    {
        // Event
        public const int EventNameMinLength = 5;
        public const int EventNameMaxLength = 20;

        public const int EventDescMinLength = 15;
        public const int EventDescMaxLength = 150;

        public const string EventInputErrorMsg = "Field {0} should be between {2} and {1} characters long";

        public const string DateTimeFormat = "yyyy-MM-dd H:mm";
        public const string DateTimeMessage = "Date and time should be in format yyyy-MM-dd H:mm";
        public const string DateTimeRegex = @"^\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}$";
        public const string InvalidDateMsg = "Invalid date";
        public const string EndBeforeStartError = "End date should not be before the start date";

        // Type
        public const int TypeNameMinLength = 5;
        public const int TypeNameMaxLength = 15;

        public const string NotExistingType = "Type does not exist";
    }
}
