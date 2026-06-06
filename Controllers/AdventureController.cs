using Models;
using Models.Enums;
using Views;

namespace Controllers
{
    /// <summary>
    /// creates adventures for hosts and handles viewing / booking / attending / reviewing for customers.
    /// This controller is shared between CustomerController and AdventureHostController.
    /// </summary>
    public class AdventureController
    {
        // Dependencies

        /// <summary>All registered users (used to build the global adventure list).</summary>
        private readonly List<User> _users;

        // Constructor

        /// <summary>Initialises the controller with the application's shared user list.</summary>
        public AdventureController(List<User> users)
        {
            _users = users;
        }

        // Adventure retrieval

        /// <summary>
        /// Collects all adventures from every registered adventure host and returns
        /// them sorted by start time (earliest first).
        /// </summary>
        public List<Adventure> GetAllAdventuresSorted()
        {
            var all = new List<Adventure>();
            foreach (User user in _users)
            {
                if (user is AdventureHost host)
                    all.AddRange(host.Adventures);
            }
            all.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            return all;
        }

        
        /// <summary>
        /// Handles the 'View all adventures' menu option for a customer.
        /// Shows the full list and optionally drills into a single adventure's details.
        /// </summary>
        public void HandleViewAll(Customer customer, CustomerView view)
        {
            List<Adventure> all = GetAllAdventuresSorted();
            bool displayed = view.DisplayAllAdventures(customer.Name, all);

            if (!displayed) return;

            int chosen = view.AskForDetails(all.Count);
            if (chosen > 0)
                view.PrintAdventureDetails(all[chosen - 1]);
        }

        // Customer: book

        /// <summary>
        /// Handles the 'Book an adventure' menu option for a customer or golden customer.
        /// </summary>
        public void HandleBook(Customer customer, CustomerView view)
        {
            List<Adventure> bookable = customer.GetBookableAdventures(GetAllAdventuresSorted());

            int adventureIndex = view.DisplayBookableAdventures(customer.Name, bookable);
            if (adventureIndex == 0) return; // nothing to book

            Adventure chosen = bookable[adventureIndex - 1];

            // A full adventure is still listed, but cannot be booked.
            if (chosen.TicketsRemaining <= 0)
            {
                view.PrintAllTicketsBooked();
                return;
            }

            // How many companions?
            int companionCount = view.ReadCompanionCount(chosen.TicketsRemaining);
            if (companionCount < 0) return; // not enough tickets – abort

            // Collect companion details
            List<(string Name, int Age)> companions =
                view.ReadCompanions(companionCount, chosen.IsOver18Only);

            int totalPeople = 1 + companionCount;

            // Determine payment method (golden customer only)
            bool usedPoints = false;
            if (customer is GoldenCustomer gc)
            {
                bool wantsPoints = view.AskUseLoyaltyPoints(chosen, totalPeople);
                if (wantsPoints)
                {
                    if (gc.CanAffordWithPoints(chosen.CostLoyaltyPoints, totalPeople))
                    {
                        gc.SpendLoyaltyPoints(chosen.CostLoyaltyPoints, totalPeople);
                        usedPoints = true;
                    }
                    else
                    {
                        // Show warning but booking still proceeds with cash
                        ((GoldenCustomerView)view).PrintInsufficientPointsWarning();
                    }
                }
            }

            customer.BookAdventure(chosen, usedPoints, companions);
            view.PrintBookingSuccess(customer.Name, chosen.Name);
        }

        // Customer: go on

        /// <summary>
        /// Handles the 'Go on an adventure' menu option.
        /// If the customer confirms, the earliest pending adventure is completed.
        /// Golden customers receive their loyalty points reward.
        /// </summary>
        public void HandleGoOn(Customer customer, CustomerView view)
        {
            List<Booking> pending = customer.GetPendingBookings();

            bool confirmed = view.DisplayPendingBookings(customer.Name, pending);
            if (!confirmed) return; // false = no adventures OR user declined

            Adventure completed = customer.GoOnNextAdventure();
            if (completed == null) return;

            view.PrintGoOnSuccess(customer.Name, completed.Name);

            // Award loyalty points to golden customers
            if (customer is GoldenCustomer gc && view is GoldenCustomerView gcv)
            {
                gc.AwardLoyaltyPoints(completed.LoyaltyPointsRewarded);
                gcv.PrintPointsAwarded(completed.LoyaltyPointsRewarded);
            }
        }

        // Customer: feedback

        /// <summary>
        /// Handles the 'Give feedback on an adventure' menu option.
        /// </summary>
        public void HandleFeedback(Customer customer, CustomerView view)
        {
            List<Booking> feedbackable = customer.GetFeedbackableBookings();

            Booking target = view.SelectFeedbackBooking(customer.Name, feedbackable);
            if (target == null) return;

            int rating = view.ReadFeedbackRating();
            string text = view.ReadFeedbackText();
            customer.GiveFeedback(target, rating, text);
        }

        // Adventure host: create

        /// <summary>
        /// Handles the 'Create a new adventure' menu option for an adventure host.
        /// </summary>
        public void HandleCreateAdventure(AdventureHost host, AdventureHostView view)
        {
            string name        = view.ReadAdventureName();
            string description = view.ReadAdventureDescription();
            var category       = view.ReadCategory();
            var ageRange       = view.ReadAgeRange();

            // The "18+ only" question is only meaningful when the age range can contain Adults
            // If they are under 18, then the question is skipped and the flag defaults false.
            bool isOver18 = false;
            if (ageRange == AgeRange.Adult || ageRange == AgeRange.Mature)
            {
                isOver18 = view.ReadIsOver18Only();
            }

            int maxTickets     = view.ReadMaxTickets();
            int costMoney      = view.ReadMoneyCost();
            int costPoints     = view.ReadLoyaltyCost();
            DateTime startTime = view.ReadStartTime();
            DateTime endTime   = view.ReadEndTime(startTime);
            int rewardPoints   = view.ReadLoyaltyReward();

            Adventure newAdventure = host.CreateAdventure(
                name, description, category, ageRange, isOver18,
                maxTickets, costMoney, costPoints, rewardPoints,
                startTime, endTime);

            view.PrintAdventureCreated(host.Name, newAdventure.Name);
        }
    }
}