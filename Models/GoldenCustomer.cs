namespace Models
{
    /// <summary>
    /// Represents a golden customer
    /// They are able to pay with loyalty points and earn rewards upon completing adventures.
    /// </summary>
    public class GoldenCustomer : Customer
    {
        // Validation constants
        /// <summary>Minimum valid loyalty number.</summary>
        public const int MinLoyaltyNumber = 100000;

        /// <summary>Maximum valid loyalty number.</summary>
        public const int MaxLoyaltyNumber = 999999;

        /// <summary>Minimum valid accrued loyalty points at registration.</summary>
        public const int MinLoyaltyPoints = 0;

        /// <summary>Maximum valid accrued loyalty points at registration.</summary>
        public const int MaxLoyaltyPoints = 1000000;

        // Properties

        /// <summary>Gets the customer's unique loyalty programme number.</summary>
        public int LoyaltyNumber { get; }

        /// <summary>Gets the customer's current loyalty points balance.</summary>
        public int LoyaltyPoints { get; private set; }

        // Constructor

        /// <summary>
        /// Initialises a new GoldenCustomer with registration and loyalty details.
        /// </summary>
        /// <param name="name">Customer's name.</param>
        /// <param name="age">Customer's age.</param>
        /// <param name="mobile">Customer's mobile phone number.</param>
        /// <param name="email">Customer's email address.</param>
        /// <param name="password">Customer's password.</param>
        /// <param name="loyaltyNumber">Customer's loyalty programme number.</param>
        /// <param name="loyaltyPoints">Customer's initial loyalty points balance.</param>
        public GoldenCustomer(string name, int age, string mobile, string email, string password,
            int loyaltyNumber, int loyaltyPoints)
            : base(name, age, mobile, email, password)
        {
            LoyaltyNumber = loyaltyNumber;
            LoyaltyPoints = loyaltyPoints;
        }

        // Overrides

        /// <summary>Returns "Golden Customer Menu." as the menu title.</summary>
        public override string GetMenuTitle() => "Golden Customer Menu.";

        /// <summary>
        /// Returns formatted details including loyalty number and current points balance.
        /// </summary>
        public override string GetDetailsString()
        {
            return $"Name: {Name}\nAge: {Age}\nMobile phone number: {Mobile}\nEmail: {Email}\n" +
                   $"Loyalty number: {LoyaltyNumber}\nLoyalty points: {LoyaltyPoints:N0}";
        }

        // Loyalty methods
        /// <summary>
        /// Checks whether the customer has enough loyalty points to pay for the whole group.
        /// </summary>
        /// <param name="costPerPerson">Loyalty-point cost per person for the adventure.</param>
        /// <param name="totalPeople">Total number of people (booker + companions).</param>
        public bool CanAffordWithPoints(int costPerPerson, int totalPeople)
        {
            return LoyaltyPoints >= costPerPerson * totalPeople;
        }

        /// <summary>
        /// Deducts the loyalty-point cost for the whole group from the balance.
        /// </summary>
        /// <param name="costPerPerson">Loyalty-point cost per person.</param>
        /// <param name="totalPeople">Total number of people (booker + companions).</param>
        public void SpendLoyaltyPoints(int costPerPerson, int totalPeople)
        {
            LoyaltyPoints -= costPerPerson * totalPeople;
        }

        /// <summary>
        /// Adds loyalty points to the balance after completing an adventure.
        /// </summary>
        /// <param name="points">Number of points to award.</param>
        public void AwardLoyaltyPoints(int points)
        {
            LoyaltyPoints += points;
        }
    }
}