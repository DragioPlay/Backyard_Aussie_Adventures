using System.Text.RegularExpressions;

namespace Validators
{
    /// <summary>
    /// Provides static validation methods for all user-supplied input.
    /// </summary>
    public static class InputValidator
    {
        // Compiled regular expressions

        /// <summary>
        /// Name must start with a letter and contain only letters, spaces,
        /// apostrophes, or hyphens.
        /// </summary>
        private static readonly Regex NameRegex =
            new Regex(@"^[a-zA-Z][a-zA-Z '\-]*$", RegexOptions.Compiled);

        /// <summary>
        /// Company name must contain at least one letter and consist only of letters, digits, spaces, apostrophes, or hyphens.
        /// </summary>
        private static readonly Regex CompanyNameRegex =
            new Regex(@"^[a-zA-Z0-9 '\-]*[a-zA-Z][a-zA-Z0-9 '\-]*$", RegexOptions.Compiled);

        /// <summary>Company nickname must be exactly three capital letters.</summary>
        private static readonly Regex NicknameRegex =
            new Regex(@"^[A-Z]{3}$", RegexOptions.Compiled);

        /// <summary>Mobile must be exactly 10 digits beginning with 0.</summary>
        private static readonly Regex MobileRegex =
            new Regex(@"^0\d{9}$", RegexOptions.Compiled);

        // Name / personal info

        /// <summary>Returns true if the name is non-empty and contains only valid characters.</summary>
        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && NameRegex.IsMatch(name);
        }

        /// <summary>
        /// Returns true if the input parses as an integer in the user age range [18, 99].
        /// </summary>
        public static bool IsValidUserAge(string input, out int age)
        {
            if (int.TryParse(input, out age))
                return age >= 18 && age <= 99;
            age = 0;
            return false;
        }

        /// <summary>Returns true if the mobile number is exactly 10 digits with a leading 0.</summary>
        public static bool IsValidMobile(string mobile)
        {
            return !string.IsNullOrEmpty(mobile) && MobileRegex.IsMatch(mobile);
        }

        /// <summary>
        /// Returns true if the email has exactly one '@' character with at least one character on each side.
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            int atCount = 0;
            int atIndex = -1;
            for (int i = 0; i < email.Length; i++)
            {
                if (email[i] == '@') { atCount++; atIndex = i; }
            }

            return atCount == 1 && atIndex > 0 && atIndex < email.Length - 1;
        }

        /// <summary>
        /// Returns true if the password meets all requirements:
        /// at least 8 characters, one digit, one lowercase, one uppercase letter.
        /// </summary>
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8) return false;

            bool hasDigit = false, hasLower = false, hasUpper = false;
            foreach (char c in password)
            {
                if (char.IsDigit(c))   hasDigit = true;
                if (char.IsLower(c))   hasLower = true;
                if (char.IsUpper(c))   hasUpper = true;
            }

            return hasDigit && hasLower && hasUpper;
        }

        // Golden customer

        /// <summary>Returns true if the loyalty number is in the range [100000, 999999].</summary>
        public static bool IsValidLoyaltyNumber(string input, out int number)
        {
            if (int.TryParse(input, out number))
                return number >= 100000 && number <= 999999;
            number = 0;
            return false;
        }

        /// <summary>Returns true if accrued loyalty points are in the range [0, 1,000,000].</summary>
        public static bool IsValidLoyaltyPoints(string input, out int points)
        {
            if (int.TryParse(input, out points))
                return points >= 0 && points <= 1000000;
            points = 0;
            return false;
        }

        // Adventure host

        /// <summary>Returns true if the company name is non-empty with at least one letter.</summary>
        public static bool IsValidCompanyName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && CompanyNameRegex.IsMatch(name);
        }

        /// <summary>Returns true if the nickname is exactly three capital letters.</summary>
        public static bool IsValidNickname(string nickname)
        {
            return !string.IsNullOrEmpty(nickname) && NicknameRegex.IsMatch(nickname);
        }

        // Adventure fields

        /// <summary>Returns true if max tickets is in [1, 30].</summary>
        public static bool IsValidMaxTickets(string input, out int tickets)
        {
            if (int.TryParse(input, out tickets))
                return tickets >= 1 && tickets <= 30;
            tickets = 0;
            return false;
        }

        /// <summary>Returns true if the money cost is in [50, 1500].</summary>
        public static bool IsValidMoneyCost(string input, out int cost)
        {
            if (int.TryParse(input, out cost))
                return cost >= 50 && cost <= 1500;
            cost = 0;
            return false;
        }

        /// <summary>Returns true if the loyalty-points cost is in [1000, 150000].</summary>
        public static bool IsValidLoyaltyCost(string input, out int cost)
        {
            if (int.TryParse(input, out cost))
                return cost >= 1000 && cost <= 150000;
            cost = 0;
            return false;
        }

        /// <summary>Returns true if the loyalty reward is in [100, 1000].</summary>
        public static bool IsValidLoyaltyReward(string input, out int reward)
        {
            if (int.TryParse(input, out reward))
                return reward >= 100 && reward <= 1000;
            reward = 0;
            return false;
        }

        /// <summary>
        /// Returns true if the input is a valid date/time in the format HH:mm dd/MM/yyyy.
        /// </summary>
        public static bool IsValidDateTime(string input, out DateTime dt)
        {
            return DateTime.TryParseExact(
                input, "HH:mm dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out dt);
        }

        // Booking

        /// <summary>Returns true if the companion count is a non-negative integer.</summary>
        public static bool IsValidCompanionCount(string input, out int count)
        {
            if (int.TryParse(input, out count))
                return count >= 0;
            count = 0;
            return false;
        }

        /// <summary>Returns true if the companion age is in [0, 99] (non-18+ adventure).</summary>
        public static bool IsValidCompanionAge(string input, out int age)
        {
            if (int.TryParse(input, out age))
                return age >= 0 && age <= 99;
            age = 0;
            return false;
        }

        /// <summary>Returns true if the companion age is in [18, 99] (18+ adventure).</summary>
        public static bool IsValidAdultCompanionAge(string input, out int age)
        {
            if (int.TryParse(input, out age))
                return age >= 18 && age <= 99;
            age = 0;
            return false;
        }

        // Feedback

        /// <summary>Returns true if the rating is in [1, 5].</summary>
        public static bool IsValidRating(string input, out int rating)
        {
            if (int.TryParse(input, out rating))
                return rating >= 1 && rating <= 5;
            rating = 0;
            return false;
        }

        // Shared

        /// <summary>Returns true if input parses as a bool (case-insensitive).</summary>
        public static bool IsValidBool(string input, out bool value)
        {
            return bool.TryParse(input, out value);
        }

        /// <summary>Returns true if input parses as an integer in [min, max].</summary>
        public static bool IsValidMenuChoice(string input, int min, int max, out int choice)
        {
            if (int.TryParse(input, out choice))
                return choice >= min && choice <= max;
            choice = 0;
            return false;
        }
    }
}