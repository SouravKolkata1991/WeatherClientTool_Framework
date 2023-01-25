using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using WeatherClientTool_Framework.BAL;
using WeatherClientTool_Framework.Models;

namespace WeatherClientTool_Framework
{
    public class Weather_Checker
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Initialize();
            string cityName = TakeInput();
            GetWeatherDetails(cityName);
            string choice = GetChoice();
            while (choice.ToUpper() == "Y" || choice.ToUpper() == "YES")
            {
                cityName = TakeInput();
                GetWeatherDetails(cityName);
                choice = GetChoice();
            }
            _logger.Info("App console is closing as user search completed");
            _logger.Info("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        }

        public static void Initialize()
        {
            Console.WriteLine("                        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            Console.WriteLine("                        X                                                         X");
            Console.WriteLine("                        X                  Custom Weather Client Tool             X");
            Console.WriteLine("                        X                                                         X");
            Console.WriteLine("                        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n\n\n");
            _logger.Info("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            _logger.Info("App initialized with console screen");


        }

        public static string WeatherCodeInterpretation(int weatherCode)
        {
            switch (weatherCode)
            {
                case 0:
                    return "Clear sky";
                case 1:
                    return "Mainly clear";
                case 2:
                    return "partly cloudy";
                case 3:
                    return "overcast";
                case 45:
                    return "Fog";
                case 48:
                    return "depositing rime fog";
                case 51:
                    return "Drizzle: Light";
                case 53:
                    return "Drizzle: moderate";
                case 55:
                    return "Drizzle: dense intensity";
                case 56:
                    return "Freezing Drizzle: Light";
                case 57:
                    return "Freezing Drizzle: dense intensity";
                case 61:
                    return "Rain: Slight";
                case 63:
                    return "Rain: moderate";
                case 65:
                    return "Rain: heavy intensity";
                case 66:
                    return "Freezing Rain: Light";
                case 67:
                    return "Freezing Rain: heavy intensity";
                case 71:
                    return "Snow fall: Slight";
                case 73:
                    return "RaSnow fallin: moderate";
                case 75:
                    return "Snow fall: heavy intensity";
                case 77:
                    return "Snow grains";
                case 80:
                    return "Rain showers: Slight";
                case 81:
                    return "Rain showers: moderate";
                case 82:
                    return "Rain showers: violent";
                case 85:
                    return "Snow showers slight";
                case 86:
                    return "Snow showers heavy";
                case 95:
                    return "Thunderstorm: Slight or moderate";
                case 96:
                    return "Thunderstorm with slight hail";
                case 99:
                    return "Thunderstorm with heavy hail";
                default:
                    return "";
            }
        }

        public static string TakeInput()
        {
            Console.WriteLine("Please enter city name:\n");
            _logger.Info("Awaiting user prompt for city name");
            string cityName = Console.ReadLine();
            return cityName;
        }

        public static string GetChoice()
        {
            Console.WriteLine("\nDo you want to search for another city (Y/N):");
            string choice = "Y";
            string userEntry = Console.ReadLine();
            bool correct_Input = false;
            while (correct_Input == false)
            {
                if (userEntry.ToUpper() == "Y" || userEntry.ToUpper() == "N" || userEntry.ToUpper() == "YES" || userEntry.ToUpper() == "NO")
                {
                    choice = userEntry;
                    correct_Input = true;
                }
                else
                {
                    Console.WriteLine("\nThis is an invalid entry");
                    Console.WriteLine("\nDo you want to search for another city (Y/N):");
                    userEntry = Console.ReadLine();
                }
            }
            return choice;
        }

        public static bool GetWeatherDetails(string cityName)
        {
            City city = null;
            
            City_Weather city_Weather = new City_Weather();
                city = city_Weather.GetWeatherDetails(cityName);
                if (city.Status == false)
                {
                    Console.WriteLine(city.Message);
                }
                else
                {
                    Console.WriteLine("\n\n");
                    Console.WriteLine("City Name: " + city.CityName);
                    Console.WriteLine("\nTemparature: " + city.Temperature + "°C");
                    Console.WriteLine("\nWindspeed: " + city.Windspeed + "kmph");
                    Console.WriteLine("\nWind Direction: " + city.WindDirection + "°");
                    Console.WriteLine("\nWeather Code: " + city.WeatherCode + " - " + WeatherCodeInterpretation(city.WeatherCode));
                }
                
       
            
            return city.Status;
        }
    }
}
