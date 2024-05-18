using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace SpaceportLaunchOptimizer;

public static class SpacePortEngineHelper
{
    public static IEnumerable<string> Spaceports = new List<string>()
        {
            "Kodiak",
            "CapeCanaveral",
            "Kourou",
            "Tanegashima",
            "Mahia"
        };

    public static Dictionary<string, WeatherData> ExecuteWeatherForecastCheck(Dictionary<string, IEnumerable<WeatherData>> data)
    {
        Dictionary<string, WeatherData> spaceportWeatherForecast = new Dictionary<string, WeatherData>();

        foreach (var (spaceport, weatherData) in data)
        {
            var weatherReport = GetBestWeatherData(weatherData);
            spaceportWeatherForecast.Add(spaceport, weatherReport);
        }

        return spaceportWeatherForecast;
    }

    public static Dictionary<string, WeatherData> ExecuteWeatherFilter(Dictionary<string, WeatherData> data)
    {
        var filteredData = data.OrderBy(x => x.Value.WindSpeed).ThenBy(x => x.Value.Humidity).ToDictionary();

        return filteredData;
    }

    public static void SendSpaceportWeatherDataReport(string senderEmail, string password, string receiverEmail, Dictionary<string, WeatherData> resultWeatherForecast, KeyValuePair<string, WeatherData> resultBestWeatherForecast)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (TextWriter tw = new StreamWriter(ms))
            using (CsvWriter csv = new CsvWriter(tw, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(resultWeatherForecast);
                tw.Flush();
                ms.Seek(0, SeekOrigin.Begin);

                try
                {
                    EmailManager.SendEmailWithAttachment(senderEmail, password, receiverEmail, new Attachment(ms, "LaunchAnalysisReport.csv"), resultBestWeatherForecast);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
    }

    public static Dictionary<string, IEnumerable<WeatherData>> GetSpaceportWeatherData(string[] filePaths)
    {
        var spaceportWeatherData = new Dictionary<string, IEnumerable<WeatherData>>();

        foreach (string file in filePaths)
        {
            var records = new List<WeatherData>();

            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", Encoding = Encoding.UTF8 }))
            {
                records = csv.GetRecords<WeatherData>().ToList();
            }

            var matchingSpaceport = Spaceports.FirstOrDefault(spaceport =>
            Path.GetFileNameWithoutExtension(file).IndexOf(spaceport, StringComparison.OrdinalIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(matchingSpaceport))
            {
                spaceportWeatherData.Add(matchingSpaceport, records);
            }
        }

        return spaceportWeatherData;
    }

    private static WeatherData GetBestWeatherData(IEnumerable<WeatherData> data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(WeatherData), "Cannot filter weather data: input is null.");
        }
        
        var weatherData = new List<WeatherData>();

        try
        {
            weatherData = data.
            Where(data =>
                data.Temperature >= 1 && data.Temperature <= 32 &&
                data.WindSpeed <= 11 &&
                data.Humidity < 55 &&
                data.Precipitation == 0 &&
                data.Lightning.ToLower() == "no" &&
                !data.Clouds.ToLower().Contains("cumulus") &&
                !data.Clouds.ToLower().Contains("nimbus"))
            .OrderBy(data => data.WindSpeed)
            .ThenBy(data => data.Humidity)
            .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while filtering and sorting weather data: " + ex.Message);
            throw;
        }

        return weatherData.FirstOrDefault()!;
    }
}
