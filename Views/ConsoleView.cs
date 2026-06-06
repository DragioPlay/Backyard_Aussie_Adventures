using Validators;

namespace Views
{
    /// <summary>
    /// Base view class providing shared console I/O helpers used by all specialised views.
    /// All Console.ReadLine / Console.WriteLine calls are routed through this layer.
    /// </summary>
    public class ConsoleView
    {
        // Display constants

        /// <summary>The border string used around error messages.</summary>
        protected const string ErrorBorder = "#####";

        /// <summary>Date/time format string used throughout the application.</summary>
        protected const string DateTimeFormat = "HH:mm dd/MM/yyyy";

        // Error output

        /// <summary>
        /// Prints a formatted error block.  The 'Please try again.' line is included by default but can be suppressed for login-style errors.
        /// </summary>
        /// <param name="message">The error message text.</param>
        /// <param name="showRetry">Whether to include '# Please try again.'</param>
        public void PrintError(string message, bool showRetry = true)
        {
            Console.WriteLine(ErrorBorder);
            Console.WriteLine($"# Error - {message}");
            if (showRetry) Console.WriteLine("# Please try again.");
            Console.WriteLine(ErrorBorder);
        }

        // Input helpers

        /// <summary>
        /// keeps validation consistent with the expected output.
        /// Reads a single line from standard input and trims surrounding whitespace.
        /// </summary>
        public string ReadLine() => (Console.ReadLine() ?? string.Empty).Trim();

        /// <summary>Prints one blank line to the console.</summary>
        public void PrintBlankLine() => Console.WriteLine();

        /// <summary>
        /// Prints the "Please enter a choice between X and Y:" prompt, then repeatedly reads input until a valid integer in [min, max] is provided.
        /// </summary>
        public int ReadMenuChoice(int min, int max)
        {
            Console.WriteLine($"Please enter a choice between {min} and {max}:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidMenuChoice(input, min, max, out int choice))
                    return choice;
                PrintError("Supplied value is out of range.");
                Console.WriteLine($"Please enter a choice between {min} and {max}:");
            }
        }

        /// <summary>
        /// Prints the given prompt then reads until a valid true/false value is supplied.
        /// On invalid input shows an error and reprints the prompt.
        /// </summary>
        public bool ReadBool(string prompt)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidBool(input, out bool value))
                    return value;
                PrintError("Supplied value is invalid.");
                Console.WriteLine(prompt);
            }
        }

        // Password helpers which are shared between registration and change-password

        /// <summary>
        /// Reads a new password for registration.
        /// Prints the requirements block after every read attempt and re-prompts on failure.
        /// </summary>
        public string ReadNewPassword()
        {
            Console.WriteLine("Please enter in your password:");
            while (true)
            {
                string input = ReadLine();
                // Requirements are always printed immediately after reading
                Console.WriteLine("Your password must:");
                Console.WriteLine("- be at least 8 characters long");
                Console.WriteLine("- contain a number");
                Console.WriteLine("- contain a lowercase letter");
                Console.WriteLine("- contain an uppercase letter");

                if (InputValidator.IsValidPassword(input))
                    return input;

                PrintError("Supplied password is invalid.");
                Console.WriteLine("Please enter in your password:");
            }
        }

        // Details display

        /// <summary>Prints the standard 'Your details.' header followed by the details string.</summary>
        public void PrintDetails(string detailsString)
        {
            Console.WriteLine("Your details.");
            Console.WriteLine(detailsString);
        }

        // Welcome

        /// <summary>Prints the application header banner.</summary>
        public void PrintAppHeader()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("Welcome to the Aussie Backyard Adventures App");
            Console.WriteLine("=============================================");
        }

        // farewell

        /// <summary>Prints the farewell message when the user exits.</summary>
        public void PrintGoodbye()
        {
            Console.WriteLine("Time to hit the road. Goodbye.");
        }

        // login

        /// <summary>Prints the 'Log in Menu.' header.</summary>
        public void PrintLoginHeader()
        {
            Console.WriteLine("Log in Menu.");
        }

        /// <summary>Prints the 'Welcome back (name).' message after a successful login.</summary>
        public void PrintWelcomeBack(string name)
        {
            Console.WriteLine($"Welcome back {name}.");
        }
    }
}