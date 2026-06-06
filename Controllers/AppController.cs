using Models;
using Views;

namespace Controllers
{
    /// <summary>
    /// Top-level application controller.
    /// Owns the shared user list, shows the main menu, and dispatches to the
    /// correct sub-controller based on the type of the logged-in user.
    /// </summary>
    public class AppController
    {
        // Shared state

        /// <summary>
        /// All users registered in the current session.
        /// </summary>
        private readonly List<User> _users;

        // Dependent controllers

        private readonly UserController _userController;
        private readonly AdventureController _adventureController;

        // View (for header / goodbye)

        private readonly ConsoleView _consoleView;

        // Constructor

        /// <summary>
        /// Builds all controllers and wires up shared dependencies.
        /// </summary>
        public AppController()
        {
            _users               = new List<User>();
            _userController      = new UserController(_users);
            _adventureController = new AdventureController(_users);
            _consoleView         = new ConsoleView();
        }

        // Main loop

        /// <summary>
        /// Prints the application banner and runs the main menu loop until
        /// the user chooses to exit.
        /// </summary>
        public void Run()
        {
            _consoleView.PrintAppHeader();

            bool running = true;
            while (running)
            {
                int choice = ShowMainMenu();

                switch (choice)
                {
                    case 1:
                        HandleLogin();
                        break;
                    case 2:
                        _userController.Register();
                        break;
                    case 3:
                        running = false;
                        break;
                }
            }

            _consoleView.PrintGoodbye();
        }

        // Main menu

        /// <summary>
        /// Prints the main menu and returns the validated choice.
        /// </summary>
        private int ShowMainMenu()
        {
            _consoleView.PrintBlankLine();
            Console.WriteLine("Please make a choice from the menu below.");
            Console.WriteLine("1. Log in as a registered user.");
            Console.WriteLine("2. Register as a new user.");
            Console.WriteLine("3. Exit.");
            return _consoleView.ReadMenuChoice(1, 3);
        }

        // Login dispatch

        /// <summary>
        /// Runs the login flow and, on success, dispatches the authenticated user to the appropriate sub-controller based on their concrete type.
        /// </summary>
        private void HandleLogin()
        {
            User user = _userController.Login();
            if (user == null) return; // no users registered – error already shown

            _consoleView.PrintWelcomeBack(user.Name);

            // Dispatch using type hierarchy:
            // GoldenCustomer is checked before Customer because it is a Customer.
            if (user is GoldenCustomer goldenCustomer)
            {
                var view = new GoldenCustomerView();
                var controller = new CustomerController(view, _adventureController);
                controller.Run(goldenCustomer);
            }
            else if (user is Customer customer)
            {
                var view = new CustomerView();
                var controller = new CustomerController(view, _adventureController);
                controller.Run(customer);
            }
            else if (user is AdventureHost host)
            {
                var view = new AdventureHostView();
                var controller = new AdventureHostController(view, _adventureController);
                controller.Run(host);
            }
        }
    }
}