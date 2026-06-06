using Models.Enums;

namespace Models
{
    /// <summary>
    /// Represents a bookable adventure created by an adventure host.
    /// </summary>
    public class Adventure
    {
        // Validation constants
        /// <summary>Minimum number of tickets an adventure can offer.</summary>
        public const int MinTickets = 1;

        /// <summary>Maximum number of tickets an adventure can offer.</summary>
        public const int MaxTickets = 30;

        /// <summary>Minimum money cost in dollars.</summary>
        public const int MinCostMoney = 50;

        /// <summary>Maximum money cost in dollars.</summary>
        public const int MaxCostMoney = 1500;

        /// <summary>Minimum loyalty-points cost.</summary>
        public const int MinCostPoints = 1000;

        /// <summary>Maximum loyalty-points cost.</summary>
        public const int MaxCostPoints = 150000;

        /// <summary>Minimum loyalty points rewarded upon completion.</summary>
        public const int MinRewardPoints = 100;

        /// <summary>Maximum loyalty points rewarded upon completion.</summary>
        public const int MaxRewardPoints = 1000;

        // Properties

        /// <summary>Gets the unique code for this adventure (e.g. AJC001).</summary>
        public string Code { get; }

        /// <summary>Gets the name of the adventure.</summary>
        public string Name { get; }

        /// <summary>Gets the description of the adventure.</summary>
        public string Description { get; }

        /// <summary>Gets the category this adventure belongs to.</summary>
        public AdventureCategory Category { get; }

        /// <summary>Gets the suitable age range for participants.</summary>
        public AgeRange AgeRange { get; }

        /// <summary>Gets whether participation is restricted to people aged 18 and over.</summary>
        public bool IsOver18Only { get; }

        /// <summary>Gets the maximum number of tickets available.</summary>
        public int MaxTicketCount { get; }

        /// <summary>Gets the current number of booked tickets.</summary>
        public int BookedTickets { get; private set; }

        /// <summary>Gets the number of tickets still available for booking.</summary>
        public int TicketsRemaining => MaxTicketCount - BookedTickets;

        /// <summary>Gets the money cost in dollars.</summary>
        public int CostMoney { get; }

        /// <summary>Gets the loyalty-points cost per person.</summary>
        public int CostLoyaltyPoints { get; }

        /// <summary>Gets the loyalty points awarded to a golden customer upon completion.</summary>
        public int LoyaltyPointsRewarded { get; }

        /// <summary>Gets the date and time the adventure starts.</summary>
        public DateTime StartTime { get; }

        /// <summary>Gets the date and time the adventure ends.</summary>
        public DateTime EndTime { get; }

        /// <summary>Gets the list of feedback entries submitted for this adventure.</summary>
        public List<Feedback> FeedbackList { get; }

        /// <summary>Gets whether at least one piece of feedback exists for this adventure.</summary>
        public bool HasFeedback => FeedbackList.Count > 0;

        // Constructor

        /// <summary>
        /// Initialises a new Adventure with all required details.
        /// </summary>
        public Adventure(string code, string name, string description,
            AdventureCategory category, AgeRange ageRange, bool isOver18Only,
            int maxTickets, int costMoney, int costLoyaltyPoints,
            int loyaltyPointsRewarded, DateTime startTime, DateTime endTime)
        {
            Code = code;
            Name = name;
            Description = description;
            Category = category;
            AgeRange = ageRange;
            IsOver18Only = isOver18Only;
            MaxTicketCount = maxTickets;
            BookedTickets = 0;
            CostMoney = costMoney;
            CostLoyaltyPoints = costLoyaltyPoints;
            LoyaltyPointsRewarded = loyaltyPointsRewarded;
            StartTime = startTime;
            EndTime = endTime;
            FeedbackList = new List<Feedback>();
        }

        // Methods

        /// <summary>
        /// Increases the booked ticket count by the given amount.
        /// </summary>
        /// <param name="count">Number of tickets to book (booker + companions).</param>
        public void BookTickets(int count)
        {
            BookedTickets += count;
        }

        /// <summary>Appends a feedback entry to this adventure's feedback list.</summary>
        /// <param name="feedback">The feedback to add.</param>
        public void AddFeedback(Feedback feedback)
        {
            FeedbackList.Add(feedback);
        }

        /// <summary>
        /// Calculates the average rating across all submitted feedback.
        /// </summary>
        /// <returns>Average rating, or 0.0 if no feedback exists.</returns>
        public double GetAverageRating()
        {
            if (FeedbackList.Count == 0) return 0.0;

            double total = 0;
            foreach (Feedback f in FeedbackList)
                total += f.Rating;

            return total / FeedbackList.Count;
        }
    }
}