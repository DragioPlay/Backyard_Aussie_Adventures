using Models.Enums;

namespace Models
{
    /// <summary>
    /// Represents an adventure host who creates and manages adventures for customers.
    /// Each host owns a sequentially numbered set of adventures identified by their
    /// company nickname.
    /// </summary>
    public class AdventureHost : User
    {
        // Private state

        /// <summary>All adventures created by this host.</summary>
        private readonly List<Adventure> _adventures;

        /// <summary>Running counter used to generate sequential adventure codes.</summary>
        private int _adventureCounter;

        // Properties

        /// <summary>Gets the company trading name of this host.</summary>
        public string CompanyName { get; }

        /// <summary>Gets the 3-capital-letter nickname used in adventure codes.</summary>
        public string CompanyNickname { get; }

        /// <summary>Gets a read-only view of all adventures created by this host.</summary>
        public IReadOnlyList<Adventure> Adventures => _adventures.AsReadOnly();

        /// <summary>Gets the total number of adventures created by this host.</summary>
        public int AdventureCount => _adventures.Count;

        // Constructor

        /// <summary>
        /// Initialises a new AdventureHost with registration and company details.
        /// </summary>
        public AdventureHost(string name, int age, string mobile, string email, string password,
            string companyName, string companyNickname)
            : base(name, age, mobile, email, password)
        {
            CompanyName = companyName;
            CompanyNickname = companyNickname;
            _adventures = new List<Adventure>();
            _adventureCounter = 0;
        }

        // Overrides

        /// <summary>Returns "Adventure Host Menu." as the menu title.</summary>
        public override string GetMenuTitle() => "Adventure Host Menu.";

        /// <summary>Returns formatted details including company info and adventure count.</summary>
        public override string GetDetailsString()
        {
            return $"Name: {Name}\nAge: {Age}\nCompany name: {CompanyName}\n" +
                   $"Company nickname: {CompanyNickname}\nMobile phone number: {Mobile}\n" +
                   $"Email: {Email}\nNumber of adventures: {AdventureCount}";
        }

        // Adventure management

        /// <summary>
        /// Creates a new adventure, assigns it the next sequential code, and
        /// adds it to this host's adventure list.
        /// </summary>
        /// <returns>The newly created Adventure.</returns>
        public Adventure CreateAdventure(
            string name, string description,
            AdventureCategory category, AgeRange ageRange, bool isOver18Only,
            int maxTickets, int costMoney, int costLoyaltyPoints,
            int loyaltyPointsRewarded, DateTime startTime, DateTime endTime)
        {
            _adventureCounter++;
            // Code = nickname + zero-padded 3-digit counter  e.g. AJC001
            string code = $"{CompanyNickname}{_adventureCounter:D3}";

            var adventure = new Adventure(
                code, name, description, category, ageRange, isOver18Only,
                maxTickets, costMoney, costLoyaltyPoints,
                loyaltyPointsRewarded, startTime, endTime);

            _adventures.Add(adventure);
            return adventure;
        }
    }
}