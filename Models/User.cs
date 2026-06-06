namespace Models
{
    /// <summary>
    /// Abstract base class for all users of the Aussie Backyard Adventures application.
    /// Holds common identity and authentication data shared by every user type.
    /// </summary>
    public abstract class User
    {
        // Validation constants
        /// <summary>Minimum valid age for a registered user.</summary>
        public const int MinAge = 18;

        /// <summary>Maximum valid age for a registered user.</summary>
        public const int MaxAge = 99;

        // Properties

        /// <summary>Gets the user's full name.</summary>
        public string Name { get; }

        /// <summary>Gets the user's age.</summary>
        public int Age { get; }

        /// <summary>Gets the user's mobile phone number.</summary>
        public string Mobile { get; }

        /// <summary>Gets the user's email address (used as unique login identifier).</summary>
        public string Email { get; }

        /// <summary>Stores the user's current password (plain-text for prototype).</summary>
        private string _password;

        // Constructor

        /// <summary>
        /// Initialises a new User with the given registration details.
        /// </summary>
        protected User(string name, int age, string mobile, string email, string password)
        {
            Name = name;
            Age = age;
            Mobile = mobile;
            Email = email;
            _password = password;
        }

        // Abstract members

        /// <summary>
        /// Returns the display title for this user type's menu
        /// </summary>
        public abstract string GetMenuTitle();

        /// <summary>
        /// Returns a formatted multi-line string of this user's details
        /// </summary>
        public abstract string GetDetailsString();

        // Authentication helpers

        /// <summary>
        /// Checks whether the supplied string matches the stored password.
        /// </summary>
        /// <param name="input">The password to verify.</param>
        public bool PasswordMatches(string input) => _password == input;

        /// <summary>
        /// Replaces the stored password with a new one.
        /// </summary>
        /// <param name="newPassword">The validated new password.</param>
        public void UpdatePassword(string newPassword)
        {
            _password = newPassword;
        }
    }
}