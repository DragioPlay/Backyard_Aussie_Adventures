using Models;
using Models.Enums;
using Validators;

namespace Views
{
    /// <summary>
    /// Handles all console I/O for adventure hosts: menus, adventure creation and viewing adventures with feedback.
    /// </summary>
    public class AdventureHostView : ConsoleView
    {
        // Enum display helpers (reuse from the CustomerView)

        /// <summary>Returns the display name for an adventure category.</summary>
        private static string GetCategoryName(AdventureCategory cat)
        {
            return CustomerView.GetCategoryName(cat);
        }

        // Menu

        /// <summary>
        /// Prints the adventure host menu and returns the validated choice.
        /// A blank line is printed before the menu title.
        /// </summary>
        public int ShowMenu(string title)
        {
            PrintBlankLine();
            Console.WriteLine(title);
            Console.WriteLine("Please make a choice from the menu below.");
            Console.WriteLine("1. See my details.");
            Console.WriteLine("2. Change password.");
            Console.WriteLine("3. Create a new adventure.");
            Console.WriteLine("4. View your adventures.");
            Console.WriteLine("5. View feedback.");
            Console.WriteLine("6. Log out.");
            return ReadMenuChoice(1, 6);
        }

        // Change password

        /// <summary>Runs the change-password flow for an adventure host.</summary>
        public string RunChangePassword(User user)
        {
            Console.WriteLine("Please enter your current password:");
            while (true)
            {
                string current = ReadLine();
                if (user.PasswordMatches(current))
                {
                    // Now read and validate the new password
                    Console.WriteLine("Please enter your new password:");
                    while (true)
                    {
                        string newPwd = ReadLine();
                        if (newPwd == current)
                        {
                            PrintError("New password is the same as the existing password.");
                            Console.WriteLine("Please enter your new password:");
                            continue;
                        }
                        if (!InputValidator.IsValidPassword(newPwd))
                        {
                            PrintError("Supplied password is invalid.");
                            Console.WriteLine("Please enter your new password:");
                            continue;
                        }
                        return newPwd;
                    }
                }
                PrintError("Entered password does not match existing password.");
                Console.WriteLine("Please enter your current password:");
            }
        }

        /// <summary>Prints the password-updated confirmation message.</summary>
        public void PrintPasswordUpdated()
        {
            Console.WriteLine("Password has been updated.");
        }

        // Adventure creation prompts

        /// <summary>Reads the adventure name (any non-empty string).</summary>
        public string ReadAdventureName()
        {
            Console.WriteLine("Please enter in the name of the adventure:");
            return ReadLine();
        }

        /// <summary>Reads the adventure description (any string).</summary>
        public string ReadAdventureDescription()
        {
            Console.WriteLine("Please enter in the description of the adventure:");
            return ReadLine();
        }

        /// <summary>Prints the category menu and reads a validated category choice.</summary>
        public AdventureCategory ReadCategory()
        {
            Console.WriteLine("Please enter the adventure category:");
            Console.WriteLine("1. Artisan");
            Console.WriteLine("2. Wildlife");
            Console.WriteLine("3. High octane!");
            Console.WriteLine("4. Education");
            Console.WriteLine("5. Hipster");
            Console.WriteLine("6. Gaming");
            int choice = ReadMenuChoice(1, 6);
            return (AdventureCategory)choice;
        }

        /// <summary>Prints the age-range menu and reads a validated age-range choice.</summary>
        public AgeRange ReadAgeRange()
        {
            Console.WriteLine("Please enter the appropriate age range:");
            Console.WriteLine("1. Juniors 0-12");
            Console.WriteLine("2. Teens 13-17");
            Console.WriteLine("3. Adults 18-64");
            Console.WriteLine("4. Mature 65-99");
            int choice = ReadMenuChoice(1, 4);
            return (AgeRange)choice;
        }

        /// <summary>Reads the 18+-only boolean flag.</summary>
        public bool ReadIsOver18Only()
        {
            return ReadBool("Please enter if the adventure is restricted only to people aged 18 and over (true/false):");
        }

        /// <summary>Reads and validates the maximum number of tickets (1–30).</summary>
        public int ReadMaxTickets()
        {
            Console.WriteLine("Please enter in the maximum number of people for the adventure between 1 and 30:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidMaxTickets(input, out int tickets))
                    return tickets;
                PrintError("Supplied value is out of range.");
                Console.WriteLine("Please enter in the maximum number of people for the adventure between 1 and 30:");
            }
        }

        /// <summary>Reads and validates the money cost ($50–$1,500).</summary>
        public int ReadMoneyCost()
        {
            Console.WriteLine("Please enter in the cost of the adventure in money between $50.00 and $1,500.00:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidMoneyCost(input, out int cost))
                    return cost;
                PrintError("Supplied value is out of range.");
                Console.WriteLine("Please enter in the cost of the adventure in money between $50.00 and $1,500.00:");
            }
        }

        /// <summary>Reads and validates the loyalty-points cost (1,000–150,000).</summary>
        public int ReadLoyaltyCost()
        {
            Console.WriteLine("Please enter in the cost of the adventure in loyalty points between 1,000 and 150,000:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidLoyaltyCost(input, out int cost))
                    return cost;
                PrintError("Supplied value is out of range.");
                Console.WriteLine("Please enter in the cost of the adventure in loyalty points between 1,000 and 150,000:");
            }
        }

        /// <summary>
        /// Reads and validates the start date/time.
        /// </summary>
        public DateTime ReadStartTime()
        {
            Console.WriteLine("Please enter in the start date and time of the adventure in HH:mm dd/MM/yyyy format:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidDateTime(input, out DateTime dt))
                    return dt;
                PrintError("Supplied start date and time of the adventure is invalid.");
                Console.WriteLine("Please enter in the start date and time of the adventure in HH:mm dd/MM/yyyy format:");
            }
        }

        /// <summary>
        /// Reads and validates the end date/time.
        /// </summary>
        public DateTime ReadEndTime(DateTime startTime)
        {
            Console.WriteLine("Please enter in the end date and time of the adventure in HH:mm dd/MM/yyyy format:");
            while (true)
            {
                string input = ReadLine();

                if (!InputValidator.IsValidDateTime(input, out DateTime dt))
                {
                    PrintError("Supplied end date and time of the adventure is invalid.");
                    Console.WriteLine("Please enter in the end date and time of the adventure in HH:mm dd/MM/yyyy format:");
                    continue;
                }

                if (dt <= startTime)
                {
                    PrintError("end date and time must be later than the start date and time");
                    Console.WriteLine("Please enter in the end date and time of the adventure in HH:mm dd/MM/yyyy format:");
                    continue;
                }

                return dt;
            }
        }

        /// <summary>Reads and validates the loyalty-points reward (100–1,000).</summary>
        public int ReadLoyaltyReward()
        {
            Console.WriteLine("Please enter in the loyalty points earned in the adventure between 100 and 1,000:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidLoyaltyReward(input, out int reward))
                    return reward;
                PrintError("Supplied value is out of range.");
                Console.WriteLine("Please enter in the loyalty points earned in the adventure between 100 and 1,000:");
            }
        }

        // Adventure creation success

        /// <summary>Prints the adventure-registered success message.</summary>
        public void PrintAdventureCreated(string hostName, string adventureName)
        {
            Console.WriteLine($"Congratulations {hostName}. You have registered your new adventure: {adventureName}.");
        }

        // View adventures table

        /// <summary>
        /// Displays the host's own adventures in the code-based table format.
        /// Shows an error if no adventures have been created.
        /// </summary>
        public void DisplayHostAdventures(IReadOnlyList<Adventure> adventures)
        {
            if (adventures.Count == 0)
            {
                PrintError("You have not created any adventures.", showRetry: false);
                return;
            }

            // Header with no row number prefix
            Console.WriteLine(
                $"{"Code",-10}{"Cost ($)",10}{"Booked Tickets",20}{"Max Tickets",15}" +
                $"{"Adventure Start",20}{"Adventure End",20}");

            foreach (Adventure a in adventures)
            {
                string cost  = $"${a.CostMoney:N2}";
                string start = a.StartTime.ToString("HH:mm dd/MM/yyyy");
                string end   = a.EndTime.ToString("HH:mm dd/MM/yyyy");

                Console.WriteLine(
                    $"{a.Code,-10}{cost,10}{a.BookedTickets,20}{a.MaxTicketCount,15}" +
                    $"    {start,-16}    {end,-16}");
            }
        }

        // View feedback table

        /// <summary>
        /// Displays all of this host's adventures with a Feedback (True/False) column,
        /// then returns the 1-based index selected.
        /// Shows an error if no adventures exist.
        /// Returns 0 on error.
        /// </summary>
        public int DisplayFeedbackTable(IReadOnlyList<Adventure> adventures)
        {
            if (adventures.Count == 0)
            {
                PrintError("Feedback has not been supplied for any adventures.", showRetry: false);
                return 0;
            }

            // Header with row numbers
            Console.WriteLine(
                $"{"",3} {"Code",-10}{"Cost ($)",10}{"Feedback",10}{"Booked Tickets",20}" +
                $"{"Max Tickets",15}{"Adventure Start",20}{"Adventure End",20}");

            for (int i = 0; i < adventures.Count; i++)
            {
                Adventure a    = adventures[i];
                string num     = $"{i + 1}.";
                string cost    = $"${a.CostMoney:N2}";
                string hasFb   = a.HasFeedback ? "True" : "False";
                string start   = a.StartTime.ToString("HH:mm dd/MM/yyyy");
                string end     = a.EndTime.ToString("HH:mm dd/MM/yyyy");

                Console.WriteLine(
                    $"{num,-3} {a.Code,-10}{cost,10}{hasFb,10}{a.BookedTickets,20}" +
                    $"{a.MaxTicketCount,15}    {start,-16}    {end,-16}");
            }

            // Ask user which adventure to inspect
            Console.WriteLine("Please enter the adventure's menu number:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidMenuChoice(input, 1, adventures.Count, out int choice))
                    return choice;
                PrintError("Supplied value is out of range.");
                Console.WriteLine("Please enter the adventure's menu number:");
            }
        }

        /// <summary>
        /// Prints the feedback detail for a selected adventure.
        /// Shows an error if no feedback exists for that adventure.
        /// </summary>
        public void PrintFeedbackDetail(Adventure adventure)
        {
            if (!adventure.HasFeedback)
            {
                PrintError("Feedback has not been supplied for this adventure.", showRetry: false);
                return;
            }

            Console.WriteLine($"Average rating = {adventure.GetAverageRating():F2}");
            for (int i = 0; i < adventure.FeedbackList.Count; i++)
            {
                Console.WriteLine($"Customer {i + 1}: {adventure.FeedbackList[i].Text}");
            }
        }
    }
}