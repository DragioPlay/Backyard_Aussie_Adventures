namespace Models
{
    /// <summary>
    /// Represents a regular customer who can view, book, attend and review adventures.
    /// </summary>
    public class Customer : User
    {
        // Private state

        /// <summary>Bookings that have not yet been attended.</summary>
        private readonly List<Booking> _pendingBookings;

        /// <summary>Bookings where the customer has gone on the adventure.</summary>
        private readonly List<Booking> _completedBookings;

        // Constructor

        /// <summary>
        /// Initialises a new Customer with core registration details.
        /// </summary>
        public Customer(string name, int age, string mobile, string email, string password)
            : base(name, age, mobile, email, password)
        {
            _pendingBookings = new List<Booking>();
            _completedBookings = new List<Booking>();
        }

        // Abstract overrides

        /// <summary>Returns "Customer Menu." as the menu title for this user type.</summary>
        public override string GetMenuTitle() => "Customer Menu.";

        /// <summary>Returns a formatted details string for a regular customer.</summary>
        public override string GetDetailsString()
        {
            return $"Name: {Name}\nAge: {Age}\nMobile phone number: {Mobile}\nEmail: {Email}";
        }

        // Booking helpers

        /// <summary>
        /// Checks whether this customer has already booked the given adventure
        /// </summary>
        public bool HasBooked(Adventure adventure)
        {
            foreach (Booking b in _pendingBookings)
                if (b.Adventure == adventure) return true;

            foreach (Booking b in _completedBookings)
                if (b.Adventure == adventure) return true;

            return false;
        }

        /// <summary>
        /// Filters the global adventure list to those the customer has not yet booked.
        /// Full adventures are still included the controller reports "All tickets have been booked." only when such an adventure is actually selected.
        /// The input list is expected to be pre-sorted by start time.
        /// </summary>
        public List<Adventure> GetBookableAdventures(List<Adventure> allAdventures)
        {
            var result = new List<Adventure>();
            foreach (Adventure a in allAdventures)
            {
                if (!HasBooked(a))
                    result.Add(a);
            }
            return result;
        }

        /// <summary>
        /// Returns pending bookings sorted by adventure start time.
        /// </summary>
        public List<Booking> GetPendingBookings()
        {
            var sorted = new List<Booking>(_pendingBookings);
            sorted.Sort((a, b) => a.Adventure.StartTime.CompareTo(b.Adventure.StartTime));
            return sorted;
        }

        /// <summary>
        /// Returns completed bookings that have not yet had feedback submitted,
        /// sorted by adventure start time.
        /// </summary>
        public List<Booking> GetFeedbackableBookings()
        {
            var result = new List<Booking>();
            foreach (Booking b in _completedBookings)
                if (!b.HasFeedback) result.Add(b);

            result.Sort((a, b) => a.Adventure.StartTime.CompareTo(b.Adventure.StartTime));
            return result;
        }

        // Actions

        /// <summary>
        /// Creates a booking for the given adventure and deducts tickets.
        /// </summary>
        /// <param name="adventure">The adventure to book.</param>
        /// <param name="usedLoyaltyPoints">Whether loyalty points were used for payment.</param>
        /// <param name="companions">Any companions travelling with the customer.</param>
        public void BookAdventure(Adventure adventure, bool usedLoyaltyPoints,
            List<(string Name, int Age)> companions)
        {
            var booking = new Booking(adventure, usedLoyaltyPoints, companions);
            // Deduct the total tickets (booker + companions)
            adventure.BookTickets(booking.TotalTickets);
            _pendingBookings.Add(booking);
        }

        /// <summary>
        /// Completes the earliest pending adventure and moves it to the completed list.
        /// </summary>
        /// <returns>The adventure that was just completed, or null if none pending.</returns>
        public Adventure GoOnNextAdventure()
        {
            List<Booking> sorted = GetPendingBookings();
            if (sorted.Count == 0) return null;

            Booking booking = sorted[0];
            booking.HasGoneOn = true;
            _pendingBookings.Remove(booking);
            _completedBookings.Add(booking);
            return booking.Adventure;
        }

        /// <summary>
        /// Records feedback for a completed booking and marks it as having feedback.
        /// </summary>
        /// <param name="booking">The completed booking to give feedback on.</param>
        /// <param name="rating">Rating from 1 to 5.</param>
        /// <param name="text">Free-text feedback.</param>
        public void GiveFeedback(Booking booking, int rating, string text)
        {
            var feedback = new Feedback(rating, text);
            booking.Adventure.AddFeedback(feedback);
            booking.MarkFeedbackGiven();
        }
    }
}