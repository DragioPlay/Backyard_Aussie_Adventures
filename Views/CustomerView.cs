using Models;
using Models.Enums;
using Validators;

namespace Views
{
    /// <summary>
    /// Handles all console I/O for a regular customer:
    /// adventure table display, booking prompts and feedback prompts.
    /// Designed to be subclassed by GoldenCustomerView to add loyalty columns.
    /// </summary>
    public class CustomerView : ConsoleView
    {
        // Enum display helpers 

        /// <summary>Returns the display name for an adventure category.</summary>
        public static string GetCategoryName(AdventureCategory cat)
        {
            switch (cat)
            {
                case AdventureCategory.Artisan:    return "Artisan"; 
                case AdventureCategory.Wildlife:   return "Wildlife";
                case AdventureCategory.HighOctane: return "High octane!";
                case AdventureCategory.Education:  return "Education";
                case AdventureCategory.Hipster:    return "Hipster";
                case AdventureCategory.Gaming:     return "Gaming";
                default:                           return cat.ToString();
            }
        }

        /// <summary>Returns the short age-range label used in table cells.</summary>
        public static string GetAgeRangeShort(AgeRange range)
        {
            switch (range)
            {
                case AgeRange.Junior: return "Juniors";
                case AgeRange.Teen:   return "Teens";
                case AgeRange.Adult:  return "Adults";
                case AgeRange.Mature: return "Mature";
                default:              return range.ToString();
            }
        }

        /// <summary>Returns the full age-range description used in detail views.</summary>
        public static string GetAgeRangeFull(AgeRange range)
        {
            switch (range)
            {
                case AgeRange.Junior: return "Juniors 0-12";
                case AgeRange.Teen:   return "Teens 13-17";
                case AgeRange.Adult:  return "Adults 18-64";
                case AgeRange.Mature: return "Mature 65-99";
                default:              return range.ToString();
            }
        }

        // Menu display

        /// <summary>
        /// Prints the customer menu and returns the user's validated choice.
        /// A blank line is printed before the menu title.
        /// </summary>
        public virtual int ShowMenu(string title)
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

        // Adventure table: View All & Book

        /// <summary>Prints the column header for the regular customer adventure table.</summary>
        protected virtual void PrintAdventureTableHeader()
        {
            Console.WriteLine(
                $"{"",3} {"Name",-25}{"Category",-15}{"Ages",-10}{"Only 18+",-10}" +
                $"{"Cost ($)",10}{"Tickets Remaining",20}{"Adventure Start",20}{"Adventure End",20}");
        }

        /// <summary>Prints a single adventure row in the regular customer table format.</summary>
        protected virtual void PrintAdventureTableRow(int index, Adventure a)
        {
            string num     = $"{index}.";
            string cost    = $"${a.CostMoney:N2}";
            string start   = a.StartTime.ToString("HH:mm dd/MM/yyyy");
            string end     = a.EndTime.ToString("HH:mm dd/MM/yyyy");

            Console.WriteLine(
                $"{num,-3}{a.Name,-25}{GetCategoryName(a.Category),-15}" +
                $"{GetAgeRangeShort(a.AgeRange),-10}{a.IsOver18Only,-10}" +
                $"{cost,10}{a.TicketsRemaining,20}    {start,-16}    {end,-16}");
        }

        /// <summary>
        /// Prints the "Displaying all adventures for NAME." message followed by the full adventure table.  If the list is empty an error is shown instead.
        /// Returns whether adventures were displayed true or false.
        /// </summary>
        public bool DisplayAllAdventures(string customerName, List<Adventure> adventures)
        {
            Console.WriteLine($"Displaying all adventures for {customerName}.");
            if (adventures.Count == 0)
            {
                // No retry line for informational "nothing to show" errors
                PrintError("There are no adventures to view.", showRetry: false);
                return false;
            }

            PrintAdventureTableHeader();
            for (int i = 0; i < adventures.Count; i++)
                PrintAdventureTableRow(i + 1, adventures[i]);

            return true;
        }

        /// <summary>
        /// After the view all table, asks whether the user wants to see details.
        /// Returns the 1 based index chosen, or 0 if the user chose false.
        /// </summary>
        public int AskForDetails(int adventureCount)
        {
            bool want = ReadBool("Please enter if you would like to see any adventure's details (true/false):");
            if (!want) return 0;

            Console.WriteLine("Please enter the adventure's menu number:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidMenuChoice(input, 1, adventureCount, out int choice))
                    return choice;
                PrintError("Supplied value is out of range.");
                Console.WriteLine("Please enter the adventure's menu number:");
            }
        }

        /// <summary>Prints a detailed view of a single adventure for a regular customer.</summary>
        public virtual void PrintAdventureDetails(Adventure a)
        {
            Console.WriteLine($"Name: {a.Name}");
            Console.WriteLine($"Description: {a.Description}");
            Console.WriteLine($"Category: {GetCategoryName(a.Category)}");
            Console.WriteLine($"Ages: {GetAgeRangeFull(a.AgeRange)}");
            Console.WriteLine($"Only 18+: {a.IsOver18Only}");
            Console.WriteLine($"Maximum number of tickets: {a.MaxTicketCount}");
            Console.WriteLine($"Money cost ($): ${a.CostMoney:N2}");
            Console.WriteLine($"Start time: {a.StartTime:HH:mm dd/MM/yyyy}");
            Console.WriteLine($"End time: {a.EndTime:HH:mm dd/MM/yyyy}");
            // No trailing blank line here — the menu loop's PrintBlankLine() provides it
        }

        // Book adventure

        /// <summary>
        /// Displays the bookable adventure list or an error if nothing is available.
        /// Returns the 1-based index of the chosen adventure, or 0 if none are available.
        /// </summary>
        public int DisplayBookableAdventures(string customerName, List<Adventure> bookable)
        {
            if (bookable.Count == 0)
            {
                PrintError("There are no adventures for you to book.", showRetry: false);
                return 0;
            }

            Console.WriteLine($"Displaying the adventures that {customerName} has not booked.");
            PrintAdventureTableHeader();
            for (int i = 0; i < bookable.Count; i++)
                PrintAdventureTableRow(i + 1, bookable[i]);

            Console.WriteLine("Please enter the adventure's menu number:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidMenuChoice(input, 1, bookable.Count, out int choice))
                    return choice;
                PrintError("Supplied value is out of range.");
                Console.WriteLine("Please enter the adventure's menu number:");
            }
        }

        /// <summary>
        /// Asks how many people are joining. Returns -1 if there are not enough tickets.
        /// Re-prompts on non-integer / negative input.
        /// </summary>
        public int ReadCompanionCount(int remainingTickets)
        {
            Console.WriteLine("Please enter the number of people joining you:");
            while (true)
            {
                string input = ReadLine();
                if (!InputValidator.IsValidCompanionCount(input, out int count))
                {
                    PrintError("Supplied value is out of range.");
                    Console.WriteLine("Please enter the number of people joining you:");
                    continue;
                }
                // Check enough tickets for booker + companions
                if (1 + count > remainingTickets)
                {
                    // No retry – return sentinel so caller can abort
                    PrintError("There are not enough tickets for your group.", showRetry: false);
                    return -1;
                }
                return count;
            }
        }

        /// <summary>
        /// Reads the name and age for each companion.
        /// Uses the correct age range based on whether the adventure is 18+ only.
        /// </summary>
        public List<(string Name, int Age)> ReadCompanions(int count, bool isOver18Only)
        {
            var companions = new List<(string Name, int Age)>();
            string agePrompt = isOver18Only
                ? "Please enter in the other person's age between 18 and 99:"
                : "Please enter in the other person's age between 0 and 99:";

            for (int i = 0; i < count; i++)
            {
                // Read companion name (any non-empty string)
                string companionName = string.Empty;
                while (string.IsNullOrEmpty(companionName))
                {
                    Console.WriteLine("Please enter in the other person's name:");
                    companionName = ReadLine();
                }

                // Read and validate companion age
                int age;
                Console.WriteLine(agePrompt);
                while (true)
                {
                    string ageInput = ReadLine();
                    bool valid = isOver18Only
                        ? InputValidator.IsValidAdultCompanionAge(ageInput, out age)
                        : InputValidator.IsValidCompanionAge(ageInput, out age);

                    if (valid) break;

                    PrintError("Supplied age is invalid.");
                    Console.WriteLine(agePrompt);
                }
                companions.Add((companionName, age));
            }
            return companions;
        }

        /// <summary>
        /// Returns false (regular customers never use loyalty points).
        /// Overridden in GoldenCustomerView to ask the prompt.
        /// </summary>
        public virtual bool AskUseLoyaltyPoints(Adventure adventure, int totalPeople)
        {
            return false;
        }

        /// <summary>Prints the booking success message.</summary>
        public void PrintBookingSuccess(string customerName, string adventureName)
        {
            Console.WriteLine($"Congratulations {customerName} you have booked a {adventureName} adventure.");
        }

        /// <summary>
        /// Prints the error shown when a customer selects a full adventure
        /// No retry line; the booking is aborted.
        /// </summary>
        public void PrintAllTicketsBooked()
        {
            PrintError("All tickets have been booked.", showRetry: false);
        }

        // Go on adventure

        /// <summary>Prints the go-on-adventure table header.</summary>
        protected void PrintGoOnTableHeader()
        {
            Console.WriteLine(
                $"{"",3} {"Name",-25}     {"Adventure Start",20}    {"Adventure End",20}");
        }

        /// <summary>Prints a row for the go-on / feedback tables.</summary>
        protected void PrintGoOnTableRow(int index, Adventure a)
        {
            string num   = $"{index}.";
            string start = a.StartTime.ToString("HH:mm dd/MM/yyyy");
            string end   = a.EndTime.ToString("HH:mm dd/MM/yyyy");
            Console.WriteLine($"{num,-3}{a.Name,-25}     {start,20}    {end,20}");
        }

        /// <summary>
        /// Shows the list of pending bookings and asks whether to go on the next adventure.
        /// Returns false if there are no pending bookings.
        /// </summary>
        public bool DisplayPendingBookings(string customerName, List<Booking> pending)
        {
            if (pending.Count == 0)
            {
                PrintError("There are no adventures for you to go on.", showRetry: false);
                return false;
            }

            Console.WriteLine($"Displaying the adventures booked by {customerName}.");
            PrintGoOnTableHeader();
            for (int i = 0; i < pending.Count; i++)
                PrintGoOnTableRow(i + 1, pending[i].Adventure);

            return ReadBool("Please enter if you would like to go on your next adventure (true/false):");
        }

        /// <summary>Prints the 'go on' success message for a regular customer.</summary>
        public virtual void PrintGoOnSuccess(string customerName, string adventureName)
        {
            Console.WriteLine($"Congratulations {customerName}, we hope you had a great time on {adventureName}.");
        }

        // Give feedback

        /// <summary>
        /// Shows the list of completed adventures awaiting feedback and lets the user
        /// pick one.  Returns null if there is nothing to give feedback on.
        /// </summary>
        public Booking SelectFeedbackBooking(string customerName, List<Booking> feedbackable)
        {
            if (feedbackable.Count == 0)
            {
                PrintError("There are no adventures for you to provide feedback.", showRetry: false);
                return null;
            }

            Console.WriteLine($"Displaying the completed adventures of {customerName}.");
            PrintGoOnTableHeader();
            for (int i = 0; i < feedbackable.Count; i++)
                PrintGoOnTableRow(i + 1, feedbackable[i].Adventure);

            Console.WriteLine("Please enter the adventure's menu number:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidMenuChoice(input, 1, feedbackable.Count, out int choice))
                    return feedbackable[choice - 1];
                PrintError("Supplied value is out of range.");
                Console.WriteLine("Please enter the adventure's menu number:");
            }
        }

        /// <summary>Reads a validated feedback rating (1–5).</summary>
        public int ReadFeedbackRating()
        {
            Console.WriteLine("Please enter in the adventure rating between 1 and 5:");
            while (true)
            {
                string input = ReadLine();
                if (InputValidator.IsValidRating(input, out int rating))
                    return rating;
                PrintError("Supplied value is out of range.");
                Console.WriteLine("Please enter in the adventure rating between 1 and 5:");
            }
        }

        /// <summary>Reads the free-text feedback comment (any non-empty string).</summary>
        public string ReadFeedbackText()
        {
            Console.WriteLine("Please enter in the text feedback:");
            return ReadLine();
        }

        // Change password

        /// <summary>
        /// Runs the change-password flow: asks for current password,
        /// then asks for new password.
        /// Returns the new password, or null if the user supplies wrong current password
        /// more than the loop allows.
        /// </summary>
        public string RunChangePassword(Models.User user)
        {
            // Step 1 – verify current password
            string current = string.Empty;
            Console.WriteLine("Please enter your current password:");
            while (true)
            {
                current = ReadLine();
                if (user.PasswordMatches(current)) break;
                PrintError("Entered password does not match existing password.");
                Console.WriteLine("Please enter your current password:");
            }

            // Step 2 – read and validate new password
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

        /// <summary>Prints the password-updated confirmation message.</summary>
        public void PrintPasswordUpdated()
        {
            Console.WriteLine("Password has been updated.");
        }
    }
}