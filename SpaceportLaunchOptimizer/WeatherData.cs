namespace SpaceportLaunchOptimizer;

public class WeatherData
{
    public WeatherData()
    {
        
    }

    public WeatherData(string date, int temperature, int windSpeed, int humidity, int precipitation, string lightning, string clouds)
    {
        Date = date;
        Temperature = temperature;
        WindSpeed = windSpeed;
        Humidity = humidity;
        Precipitation = precipitation;
        Lightning = lightning;
        Clouds = clouds;
    }

    public string Date { get; set; }
    public int Temperature { get; set; }
    public int WindSpeed { get; set; }
    public int Humidity { get; set; }
    public int Precipitation { get; set; }
    public string Lightning { get; set; }
    public string Clouds { get; set; }
}
