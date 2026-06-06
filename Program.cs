using System.Globalization;
using Controllers;

/// <summary>
/// Application entry point. Creates the top-level AppController and starts the run loop.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        // "1,000.00" / "30,000" formatting regardless of settings
        Thread.CurrentThread.CurrentCulture   = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        AppController app = new AppController();
        app.Run();
    }
}
