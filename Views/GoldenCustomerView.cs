using Models;


namespace Views
{
    /// <summary>
    /// Extends CustomerView with the additional loyalty-point columns shown in adventure tables and the prompt to pay with loyalty points when booking.
    /// </summary>
    public class GoldenCustomerView : CustomerView
    {
        // Menu

        /// <summary>
        /// Prints the golden customer menu and returns the validated choice.
        /// </summary>
        public override int ShowMenu(string title)
        {
            PrintBlankLine();
            Console.WriteLine(title);
            Console.WriteLine("Please make a choice from the menu below.");
            Console.WriteLine("1. See my details.");
            Console.WriteLine("2. Change password.");
            Console.WriteLine("3. View all adventures.");
            Console.WriteLine("4. Book an adventure.");
            Console.WriteLine("5. Go on an adventure.");
            Console.WriteLine("6. Give feedback on an adventure.");
            Console.WriteLine("7. Log out.");
            return ReadMenuChoice(1, 7);
        }

        // Adventure table with loyalty columns

        /// <summary>
        /// Prints the golden customer adventure table header, which includes points Cost and Points Reward columns between Cost and Tickets.
        /// </summary>
        protected override void PrintAdventureTableHeader()
        {
            Console.WriteLine(
                $"{"",3} {"Name",-25}{"Category",-15}{"Ages",-10}{"Only 18+",-10}" +
                $"{"Cost ($)",10}{"Points Cost",15}{"Points Reward",15}" +
                $"{"Tickets Remaining",20}{"Adventure Start",20}{"Adventure End",20}");
        }

        /// <summary>
        /// Prints a single adventure row including the two loyalty-point columns.
        /// </summary>
        protected override void PrintAdventureTableRow(int index, Adventure a)
        {
            string num    = $"{index}.";
            string cost   = $"${a.CostMoney:N2}";
            string points = $"{a.CostLoyaltyPoints:N0}";
            string reward = $"{a.LoyaltyPointsRewarded:N0}";
            string start  = a.StartTime.ToString("HH:mm dd/MM/yyyy");
            string end    = a.EndTime.ToString("HH:mm dd/MM/yyyy");

            Console.WriteLine(
                $"{num,-3}{a.Name,-25}{GetCategoryName(a.Category),-15}" +
                $"{GetAgeRangeShort(a.AgeRange),-10}{a.IsOver18Only,-10}" +
                $"{cost,10}{points,15}{reward,15}" +
                $"{a.TicketsRemaining,20}    {start,-16}    {end,-16}");
        }

        // Adventure detail view

        /// <summary>
        /// Prints a detailed adventure view for a golden customer, including
        /// the loyalty cost and reward fields.
        /// </summary>
        public override void PrintAdventureDetails(Adventure a)
        {
            Console.WriteLine($"Name: {a.Name}");
            Console.WriteLine($"Description: {a.Description}");
            Console.WriteLine($"Category: {GetCategoryName(a.Category)}");
            Console.WriteLine($"Ages: {GetAgeRangeFull(a.AgeRange)}");
            Console.WriteLine($"Only 18+: {a.IsOver18Only}");
            Console.WriteLine($"Maximum number of tickets: {a.MaxTicketCount}");
            Console.WriteLine($"Money cost ($): ${a.CostMoney:N2}");
            Console.WriteLine($"Loyalty points cost: {a.CostLoyaltyPoints:N0}");
            Console.WriteLine($"Start time: {a.StartTime:HH:mm dd/MM/yyyy}");
            Console.WriteLine($"End time: {a.EndTime:HH:mm dd/MM/yyyy}");
            Console.WriteLine($"Points awarded: {a.LoyaltyPointsRewarded:N0}");
            // No trailing blank line — the menu loop's PrintBlankLine() provides it
        }

        // Loyalty-point payment prompt

        /// <summary>
        /// Asks the golden customer whether to pay with loyalty points.
        /// If they say yes but cannot afford it, prints a warning and books with cash.
        /// Returns true only if payment with points was confirmed AND affordable.
        /// </summary>
        public override bool AskUseLoyaltyPoints(Adventure adventure, int totalPeople)
        {
            bool usePoints = ReadBool("Please enter if you want to use your loyalty points (true/false):");
            return usePoints;
        }

        /// <summary>
        /// Prints the insufficient-points warning.  The booking still proceeds with cash.
        /// </summary>
        public void PrintInsufficientPointsWarning()
        {
            Console.WriteLine(ErrorBorder);
            Console.WriteLine("# Error - Not enough loyalty points for the adventure. You will need to pay in cash.");
            Console.WriteLine(ErrorBorder);
        }

        // Go-on success with loyalty reward

        /// <summary>
        /// Prints the go-on success message followed by the loyalty points earned.
        /// </summary>
        public override void PrintGoOnSuccess(string customerName, string adventureName)
        {
            Console.WriteLine($"Congratulations {customerName}, we hope you had a great time on {adventureName}.");
        }

        /// <summary>Prints the loyalty-point award message after completing an adventure.</summary>
        public void PrintPointsAwarded(int points)
        {
            Console.WriteLine($"You have earned {points:N0} loyalty points for this adventure.");
        }
    }
}