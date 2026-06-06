using Models;
using Views;

namespace Controllers
{
    /// <summary>
    /// Manages the adventure host menu loop, handling creation and review of adventures.
    /// </summary>
    public class AdventureHostController
    {
        // Dependencies

        /// <summary>
        /// View for all adventure host console I/O.
        /// </summary>
        private readonly AdventureHostView _view;

        /// <summary>
        /// Adventure controller used for the create-adventure operation.
        /// </summary>
        private readonly AdventureController _adventureController;

        // Constructor

        /// <summary>
        /// Initialises the controller with its required dependencies.
        /// </summary>
        public AdventureHostController(AdventureHostView view, AdventureController adventureController)
        {
            _view = view;
            _adventureController = adventureController;
        }

        // Menu loop

        /// <summary>
        /// Runs the adventure host menu loop until the host chooses to log out.
        /// </summary>
        /// <param name="host">
        /// The currently logged-in adventure host.
        /// </param>
        public void Run(AdventureHost host)
        {
            bool loggedIn = true;
            while (loggedIn)
            {
                int choice = _view.ShowMenu(host.GetMenuTitle());

                switch (choice)
                {
                    case 1:
                        HandleSeeDetails(host);
                        break;
                    case 2:
                        HandleChangePassword(host);
                        break;
                    case 3:
                        _adventureController.HandleCreateAdventure(host, _view);
                        break;
                    case 4:
                        HandleViewAdventures(host);
                        break;
                    case 5:
                        HandleViewFeedback(host);
                        break;
                    case 6:
                        loggedIn = false;
                        break;
                }
            }
        }

        // Option handlers

        /// <summary>
        /// Displays the host's registration and company details.
        /// </summary>
        /// <param name="host">
        /// The adventure host whose details to display.
        /// </param>
        private void HandleSeeDetails(AdventureHost host)
        {
            _view.PrintDetails(host.GetDetailsString());
        }

        /// <summary>
        /// Runs the change-password flow for this host.
        /// </summary>
        /// <param name="host">
        /// The adventure host whose password to change.
        /// </param>
        private void HandleChangePassword(AdventureHost host)
        {
            string newPwd = _view.RunChangePassword(host);
            host.UpdatePassword(newPwd);
            _view.PrintPasswordUpdated();
        }

        /// <summary>
        /// Displays all adventures created by this host.
        /// </summary>
        /// <param name="host">
        /// The adventure host whose adventures to display.
        /// </param>
        private void HandleViewAdventures(AdventureHost host)
        {
            _view.DisplayHostAdventures(host.Adventures);
        }

        /// <summary>
        /// Displays all adventures with feedback indicators, then lets the host
        /// view detailed ratings and comments for a chosen adventure.
        /// </summary>
        /// <param name="host">
        /// The adventure host who wants to view feedback.
        /// </param>
        private void HandleViewFeedback(AdventureHost host)
        {
            int index = _view.DisplayFeedbackTable(host.Adventures);
            if (index == 0) return; // error already shown

            Adventure selected = host.Adventures[index - 1];
            _view.PrintFeedbackDetail(selected);
        }
    }
}