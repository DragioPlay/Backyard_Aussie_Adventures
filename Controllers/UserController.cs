using Models;
using Validators;

namespace Controllers
{
    /// <summary>
    /// Handles user registration and login operations.
    /// All console inputs and outputs for these flows is performed inside this controller via
    /// Console.ReadLine/WriteLine (kept separate from model logic).
    /// </summary>
    public class UserController
    {
        // Dependencies

        /// <summary>Shared list of all registered users, modified by this controller.</summary>
        private readonly List<User> _users;

        // Constructor

        /// <summary>Initialises the controller with the application's user list.</summary>
        public UserController(List<User> users)
        {
            _users = users;
        }

        // Registration

        /// <summary>
        /// Runs the full registration flow:
        /// shows user-type menu, collects and validates all fields,
        /// creates the appropriate User subclass and adds it to the list.
        /// </summary>
        /// <returns>The newly created user.</returns>
        public User Register()
        {
            int userType = ReadUserTypeMenu();

            switch (userType)
            {
                case 1: return RegisterCustomer();
                case 2: return RegisterGoldenCustomer();
                case 3: return RegisterAdventureHost();
                default: throw new InvalidOperationException("Unexpected user type.");
            }
        }

        /// <summary>Prints the user-type menu and returns the validated choice.</summary>
        private int ReadUserTypeMenu()
        {
            Console.WriteLine("Please enter the user type to register:");
            Console.WriteLine("1. A customer.");
            Console.WriteLine("2. A golden customer.");
            Console.WriteLine("3. An adventure host.");
            return ReadMenuChoice(1, 3);
        }

        /// <summary>Collects and validates fields common to all user types.</summary>
        private (string name, int age, string mobile, string email, string password) ReadCommonFields()
        {
            string name = ReadValidatedName("Please enter in your name:");

            Console.WriteLine("Please enter in your age between 18 and 99:");
            int age = ReadValidatedAge();

            string mobile = ReadValidatedMobile();

            string email = ReadValidatedEmail();

            string password = ReadNewPassword();

            return (name, age, mobile, email, password);
        }

        // Register customer

        private Customer RegisterCustomer()
        {
            Console.WriteLine("Registering as a customer.");
            var (name, age, mobile, email, password) = ReadCommonFields();
            var customer = new Customer(name, age, mobile, email, password);
            _users.Add(customer);
            Console.WriteLine($"Congratulations {name}. You have registered as a customer.");
            return customer;
        }

        // Register golden customer

        private GoldenCustomer RegisterGoldenCustomer()
        {
            var (name, age, mobile, email, password) = ReadCommonFields();

            // Loyalty number
            Console.WriteLine("Please enter in your loyalty number between 100000 and 999999:");
            int loyaltyNumber = ReadValidatedInt(
                InputValidator.IsValidLoyaltyNumber,
                "Please enter in your loyalty number between 100000 and 999999:",
                "Supplied loyalty number is invalid.");

            // Accrued loyalty points
            Console.WriteLine("Please enter in your accrued loyalty points between 0 and 1,000,000:");
            int loyaltyPoints = ReadValidatedInt(
                InputValidator.IsValidLoyaltyPoints,
                "Please enter in your accrued loyalty points between 0 and 1,000,000:",
                "Supplied accrued loyalty points are invalid.");

            var gc = new GoldenCustomer(name, age, mobile, email, password, loyaltyNumber, loyaltyPoints);
            _users.Add(gc);
            Console.WriteLine($"Congratulations {name}. You have registered as a golden customer.");
            return gc;
        }

        // Register adventure host

        private AdventureHost RegisterAdventureHost()
        {
            var (name, age, mobile, email, password) = ReadCommonFields();

            // Company name
            string companyName = ReadValidatedCompanyName();

            // Company nickname
            string nickname = ReadValidatedNickname();

            var host = new AdventureHost(name, age, mobile, email, password, companyName, nickname);
            _users.Add(host);
            Console.WriteLine($"Congratulations {name}. You have registered as an adventure host.");
            return host;
        }

        // Login

        /// <summary>
        /// Runs the login flow.  Returns the authenticated user, or null if no
        /// users are registered (caller must handle the error display).
        /// </summary>
        public User? Login()
        {
            if (_users.Count == 0)
            {
                PrintError("There are no people registered.", showRetry: false);
                return null;
            }

            Console.WriteLine("Log in Menu.");

            // Validate email
            string email = ReadLoginEmail();
            if (email == null) return null;

            // Validate password
            User user = FindUserByEmail(email);
            Console.WriteLine("Please enter in your password:");
            while (true)
            {
                string input = ReadInput();
                if (user.PasswordMatches(input)) return user;
                PrintError("Incorrect password.", showRetry: false);
                Console.WriteLine("Please enter in your password:");
            }
        }

        /// <summary>
        /// Repeatedly asks for an email until a registered one is provided.
        /// </summary>
        private string ReadLoginEmail()
        {
            Console.WriteLine("Please enter in your email:");
            while (true)
            {
                string input = ReadInput();
                User found = FindUserByEmail(input);
                if (found != null) return input;
                PrintError("Email is not registered.", showRetry: false);
                Console.WriteLine("Please enter in your email:");
            }
        }

        /// <summary>Returns the user matching the given email, or null.</summary>
        private User? FindUserByEmail(string email)
        {
            foreach (User u in _users)
                if (u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                    return u;
            return null;
        }

        /// <summary>Reads a validated user name, re-prompting on failure.</summary>
        private string ReadValidatedName(string prompt)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                string input = ReadInput();
                if (InputValidator.IsValidName(input)) return input;
                PrintError("Supplied name is invalid.");
                Console.WriteLine(prompt);
            }
        }

        /// <summary>Reads a validated user age, re-prompting on failure.</summary>
        private int ReadValidatedAge()
        {
            while (true)
            {
                string input = ReadInput();
                if (InputValidator.IsValidUserAge(input, out int age)) return age;
                PrintError("Supplied age is invalid.");
                Console.WriteLine("Please enter in your age between 18 and 99:");
            }
        }

        /// <summary>Reads a validated mobile number, re-prompting on failure.</summary>
        private string ReadValidatedMobile()
        {
            Console.WriteLine("Please enter in your mobile number:");
            while (true)
            {
                string input = ReadInput();
                if (InputValidator.IsValidMobile(input)) return input;
                PrintError("Supplied mobile number is invalid.");
                Console.WriteLine("Please enter in your mobile number:");
            }
        }

        /// <summary>Reads a validated unique email, re-prompting on failure.</summary>
        private string ReadValidatedEmail()
        {
            Console.WriteLine("Please enter in your email:");
            while (true)
            {
                string input = ReadInput();
                if (!InputValidator.IsValidEmail(input))
                {
                    PrintError("Supplied email is invalid.");
                    Console.WriteLine("Please enter in your email:");
                    continue;
                }
                if (FindUserByEmail(input) != null)
                {
                    PrintError("Email already registered.");
                    Console.WriteLine("Please enter in your email:");
                    continue;
                }
                return input;
            }
        }

        /// <summary>
        /// Reads and validates a new password with the requirements block displayed
        /// after every read attempt.
        /// </summary>
        private string ReadNewPassword()
        {
            Console.WriteLine("Please enter in your password:");
            while (true)
            {
                string input = ReadInput();
                Console.WriteLine("Your password must:");
                Console.WriteLine("- be at least 8 characters long");
                Console.WriteLine("- contain a number");
                Console.WriteLine("- contain a lowercase letter");
                Console.WriteLine("- contain an uppercase letter");

                if (InputValidator.IsValidPassword(input)) return input;

                PrintError("Supplied password is invalid.");
                Console.WriteLine("Please enter in your password:");
            }
        }

        /// <summary>Reads a validated company name, re-prompting on failure.</summary>
        private string ReadValidatedCompanyName()
        {
            Console.WriteLine("Please enter in your company name:");
            while (true)
            {
                string input = ReadInput();
                if (InputValidator.IsValidCompanyName(input)) return input;
                PrintError("Supplied company name is invalid.");
                Console.WriteLine("Please enter in your company name:");
            }
        }

        /// <summary>Reads a validated 3-capital-letter nickname, re-prompting on failure.</summary>
        private string ReadValidatedNickname()
        {
            Console.WriteLine("Please enter in your company nickname using 3 letters:");
            while (true)
            {
                string input = ReadInput();
                if (InputValidator.IsValidNickname(input)) return input;
                PrintError("Supplied company nickname using 3 letters is invalid.");
                Console.WriteLine("Please enter in your company nickname using 3 letters:");
            }
        }

        // Validated int reader

        /// <summary>
        /// Reads lines until the delegate validates successfully.
        /// The prompt is only reprinted on error (caller has already printed it once).
        /// </summary>
        private delegate bool IntValidator(string input, out int result);

        private int ReadValidatedInt(IntValidator validator, string prompt, string errorMsg)
        {
            while (true)
            {
                string input = ReadInput();
                if (validator(input, out int value)) return value;
                PrintError(errorMsg);
                Console.WriteLine(prompt);
            }
        }

        // Menu choice helper

        private int ReadMenuChoice(int min, int max)
        {
            Console.WriteLine($"Please enter a choice between {min} and {max}:");
            while (true)
            {
                string input = ReadInput();
                if (InputValidator.IsValidMenuChoice(input, min, max, out int choice))
                    return choice;
                PrintError("Supplied value is out of range.");
                Console.WriteLine($"Please enter a choice between {min} and {max}:");
            }
        }

        // Error formatting

        /// <summary>
        /// Reads a line from standard input and trims surrounding whitespace.
        /// Trimming matches the reference solution, which accepts inputs that
        /// have leading or trailing spaces (e.g. "0491373798   ").
        /// </summary>
        private string ReadInput() => (Console.ReadLine() ?? string.Empty).Trim();

        private void PrintError(string message, bool showRetry = true)
        {
            Console.WriteLine("#####");
            Console.WriteLine($"# Error - {message}");
            if (showRetry) Console.WriteLine("# Please try again.");
            Console.WriteLine("#####");
        }
    }
}