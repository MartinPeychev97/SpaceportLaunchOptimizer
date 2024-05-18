using System.Globalization;

namespace SpaceportLaunchOptimizer;

public static class ConsoleInputHelper
{
    public static void AdjustLanguage()
    {
        Console.WriteLine("Choose language:");
        Console.WriteLine("Press 1 for English");
        Console.WriteLine("Press 2 for Deutsch");

        var languageChoice = Console.ReadLine();

        while (string.IsNullOrEmpty(languageChoice))
        {
            if (languageChoice == "1")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                break;
            }
            else if (languageChoice == "2")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
                break;
            }

            languageChoice = Console.ReadLine();
        }
    }

    public static string[] ReadInput()
    {
        var folderPath = PromptUser("Enter the folder path:", "Geben Sie den Ordnerpfad ein:");
        var senderEmail = PromptUser("Enter the sender email address:", "Geben Sie die Absender-E-Mail-Adresse ein:");
        var password = PromptUser("Enter the password:", "Geben Sie das Passwort ein:");
        var receiverEmail = PromptUser("Enter the receiver email address:", "Geben Sie die Empfänger-E-Mail-Adresse ein:");

        var args = new string[] { folderPath, senderEmail, password, receiverEmail };
        return args;
    }

    private static string PromptUser(string englishMessage, string germanMessage)
    {
        if (Thread.CurrentThread.CurrentUICulture.Name == "de-DE")
            Console.WriteLine(germanMessage);
        else
            Console.WriteLine(englishMessage);
        
        var input = Console.ReadLine();

        while (string.IsNullOrEmpty(input))
        {
            if (String.IsNullOrEmpty(input))
                Console.WriteLine("No input specified/Keine Eingabe angegeben");

            input = Console.ReadLine();
        }

        return input;
    }
}
