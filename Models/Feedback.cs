namespace Models
{
    /// <summary>
    /// Represents customer feedback submitted for a completed adventure.
    /// </summary>
    public class Feedback
    {
        /// <summary>Gets the numeric rating out of 5.</summary>
        public int Rating { get; }

        /// <summary>Gets the free-text feedback comment.</summary>
        public string Text { get; }

        /// <summary>
        /// Initialises a new Feedback instance.
        /// </summary>
        /// <param name="rating">Rating between 1 and 5 inclusive.</param>
        /// <param name="text">Free-text feedback comment.</param>
        public Feedback(int rating, string text)
        {
            Rating = rating;
            Text = text;
        }
    }
}