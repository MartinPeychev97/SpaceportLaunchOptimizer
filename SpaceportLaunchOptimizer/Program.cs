using SpaceportLaunchOptimizer;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Welcome to the SPACE Programme");
        // Step 1: Ask for language preference
        ConsoleInputHelper.AdjustLanguage();

        // Step 3: Get arguments
        string[] args = ConsoleInputHelper.ReadInput();

        // Step 4: Execute the SpacePortEngine
        SpacePortEngine engine = new SpacePortEngine();
        engine.Execute(args);
    }
}
