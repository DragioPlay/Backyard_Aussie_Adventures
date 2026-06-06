using Models;
using Views;

namespace Controllers
{
    /// <summary>
    /// Manages the customer menu loop for both regular customers and golden customers.
    /// Delegates all adventure operations to AdventureController and all
    /// console I/O to the injected CustomerView or GoldenCustomerView.
    /// </summary>
    public class CustomerController
    {
        // Dependencies

        /// <summary>View used for all customer console inputs and outputs.</summary>
        private readonly CustomerView _view;

        /// <summary>Adventure controller for booking, viewing and feedback.</summary>
        private readonly AdventureController _adventureController;

        // Constructor

        /// <summary>
        /// Initialises the controller with a shared view and adventure controller.
        /// </summary>
        public CustomerController(CustomerView view, AdventureController adventureController)
        {
            _view = view;
            _adventureController = adventureController;
        }

        // Menu loop

        /// <summary>
        /// Runs the customer menu loop until the user chooses to log out.
        /// </summary>
        /// <param name="customer">The currently logged-in customer.</param>
        public void Run(Customer customer)
        {
            bool loggedIn = true;
            while (loggedIn)
            {
                int choice = _view.ShowMenu(customer.GetMenuTitle());

                switch (choice)
                {
                    case 1:
                        HandleSeeDetails(customer);
                        break;
                    case 2:
                        HandleChangePassword(customer);
                        break;
                    case 3:
                        _adventureController.HandleViewAll(customer, _view);
                        break;
                    case 4:
                        _adventureController.HandleBook(customer, _view);
                        break;
                    case 5:
                        _adventureController.HandleGoOn(customer, _view);
                        break;
                    case 6:
                        _adventureController.HandleFeedback(customer, _view);
                        break;
                    case 7:
                        loggedIn = false;
                        break;
                }
            }
        }

        // Option handlers

        /// <summary>
        /// Displays the customer's registration details.
        /// </summary>
        private void HandleSeeDetails(Customer customer)
        {
            _view.PrintDetails(customer.GetDetailsString());
        }

        /// <summary>
        /// Runs the change-password flow and applies the new password on success.
        /// </summary>
        private void HandleChangePassword(Customer customer)
        {
            string newPwd = _view.RunChangePassword(customer);
            customer.UpdatePassword(newPwd);
            _view.PrintPasswordUpdated();
        }
    }
}