namespace SpaceportLaunchOptimizer;

public class SpacePortEngine
{
    public void Execute(string[] args)
    {
        if (args.Length < 4)
        {
            Console.WriteLine("Usage: SpaceportLaunchOptimizer <FolderPath> <SenderEmailAddress> <Password> <ReceiverEmailAddress>");
            Console.WriteLine("Please ensure proper input");
            return;
        }

        var folderPath = args[0];
        var senderEmail = args[1];
        var password = args[2];
        var receiverEmail = args[3];

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Error: Directory '{folderPath}' does not exist.");
            return;
        }

        var filePaths = Directory.GetFiles(folderPath, "*.csv", SearchOption.TopDirectoryOnly);

        //Read weather data for each spaceport
        var spaceportWeatherData = SpacePortEngineHelper.GetSpaceportWeatherData(filePaths);

        //Filter weather data for each spaceport
        var resultWeatherForecast = SpacePortEngineHelper.ExecuteWeatherForecastCheck(spaceportWeatherData);
        var resultBestWeatherForecast = SpacePortEngineHelper.ExecuteWeatherFilter(resultWeatherForecast).FirstOrDefault();

        //Send email with spaceport weather report 
        SpacePortEngineHelper.SendSpaceportWeatherDataReport(senderEmail, password, receiverEmail, resultWeatherForecast, resultBestWeatherForecast);
    }
}
