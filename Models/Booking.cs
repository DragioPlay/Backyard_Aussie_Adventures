namespace Models
{
    /// <summary>
    /// Represents a customer's booking for a specific adventure,
    /// including optional companions.
    /// </summary>
    public class Booking
    {
        /// <summary>Gets the adventure that was booked.</summary>
        public Adventure Adventure { get; }

        /// <summary>Gets whether loyalty points were used to pay for this booking.</summary>
        public bool UsedLoyaltyPoints { get; }

        /// <summary>Gets or sets whether the customer has gone on this adventure.</summary>
        public bool HasGoneOn { get; set; }

        /// <summary>Gets whether feedback has been submitted for this booking.</summary>
        public bool HasFeedback { get; private set; }

        /// <summary>Gets the list of companions (name and age) joining the customer.</summary>
        public List<(string Name, int Age)> Companions { get; }

        /// <summary>
        /// Gets the total number of tickets used by this booking
        /// (One for the customer as well as the number of companions).
        /// </summary>
        public int TotalTickets => 1 + Companions.Count;

        /// <summary>
        /// Initialises a new Booking.
        /// </summary>
        /// <param name="adventure">The adventure being booked.</param>
        /// <param name="usedLoyaltyPoints">Whether loyalty points were used.</param>
        /// <param name="companions">List of companions travelling with the customer.</param>
        public Booking(Adventure adventure, bool usedLoyaltyPoints, List<(string Name, int Age)> companions)
        {
            Adventure = adventure;
            UsedLoyaltyPoints = usedLoyaltyPoints;
            HasGoneOn = false;
            HasFeedback = false;
            Companions = companions;
        }

        /// <summary>Marks this booking as having had feedback submitted.</summary>
        public void MarkFeedbackGiven()
        {
            HasFeedback = true;
        }
    }
}