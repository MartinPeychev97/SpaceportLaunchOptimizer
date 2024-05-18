
namespace SpaceportLaunchOptimizer.Tests
{
    public class SpacePortEngineHelperTests
    {
        [Test]
        public void Execute_ExecuteWeatherForecastCheck_ShouldReturnSuccesResult()
        {
            var data = new Dictionary<string, IEnumerable< WeatherData>>() {
                        {"TestPortOne", new List<WeatherData>(){ new WeatherData("1", 12, 1, 5, 0,"NO","Cirrus")} },
                        {"PortTwo", new List<WeatherData>(){ new WeatherData("2", 8, 3, 40, 0, "NO", "Stratus")} },
                        {"TestPortTwo", new List<WeatherData>(){new WeatherData("3", 8, 10, 15, 0,"Yes","Cumulus")} },
            };

            var result = SpacePortEngineHelper.ExecuteWeatherForecastCheck(data);
            
            var actualResult = result.Values.First();
            var expectedResult = data.First().Value.First();
            
            Assert.AreEqual(expectedResult, actualResult );
        }

        [Test]
        public void ExecuteWeatherFilter_ShouldReturnFilteredData()
        {
            var data = new Dictionary<string, WeatherData>()
            {
                {"PortOne", new WeatherData("1", 10, 5, 50, 0, "NO", "Cirrus")},
                {"PortTwo", new WeatherData("2", 8, 3, 40, 0, "NO", "Stratus")},
                {"PortThree", new WeatherData("3", 12, 4, 30, 0, "NO", "Nimbus")}
            };

            var result = SpacePortEngineHelper.ExecuteWeatherFilter(data);

            var expectedOrder = data.OrderBy(x => x.Value.WindSpeed).ThenBy(x => x.Value.Humidity).Select(x => x.Key).ToList();
            var actualOrder = result.Keys.ToList();

            CollectionAssert.AreEqual(expectedOrder, actualOrder);
        }

        [Test]
        public void SendSpaceportWeatherDataReport_ShouldSendEmailSuccessfully()
        {
            var resultWeatherForecast = new Dictionary<string, WeatherData>
            {
                {"PortOne", new WeatherData("1", 10, 5, 50, 0, "NO", "Cirrus")}
            };
            var resultBestWeatherForecast = new KeyValuePair<string, WeatherData>("PortOne", new WeatherData("1", 10, 5, 50, 0, "NO", "Cirrus"));

            Assert.DoesNotThrow(() => SpacePortEngineHelper.SendSpaceportWeatherDataReport("sender@example.com", "password", "receiver@example.com", resultWeatherForecast, resultBestWeatherForecast));
        }
    }
}
