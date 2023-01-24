using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherClientTool_Framework.Models;

namespace WeatherClientTool_Framework.DAL
{
    public class City_Weather_DAL
    {
        public static Logger _logger= LogManager.GetCurrentClassLogger();

        //Get Weather data from API for city name provided by user
        public City GetWeatherDetails(City _city)
        {
            _logger.Info("Getting weather details for the city: "+_city.CityName);
            string api_URL = ConfigurationManager.AppSettings["API_URL"].ToString();
            api_URL = api_URL + "?latitude=" + _city.Latitude + "&longitude=" + _city.Longtitude + "&current_weather=true";
            HttpClient client= new HttpClient();
            try
            {
                var uri = client.GetAsync(api_URL);
                while (!uri.IsCompleted)
                {
                    uri.Wait(100);
                }
                if(uri.IsCompleted)
                {
                    var result = uri.Result;
                    ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
                    _city.Temperature = apiResponse.current_Weather.temperature;
                    _city.Windspeed = apiResponse.current_Weather.windspeed;
                    _city.WindDirection = apiResponse.current_Weather.winddirection;
                    _city.WeatherCode = apiResponse.current_Weather.weathercode;
                    _city.Status = true;
                    _logger.Info("Getting weather details for the city: " + _city.CityName+" completed");
                }

            }
            catch(Exception ex)
            {
                _logger.Fatal("Error Occured while getting weather details from data layer");
                _logger.Fatal("Error message:\n" + ex.Message);
                _city.Status = false;
                _city.Message = "Error Occured while getting weather details from data layer";
            }
            return _city;
        }

        //Get Latitude and Longitude data from CSV file for city name provided by user
        public City GetLatLongDetails(string cityName)
        {
            City city = new City();

            try
            {
                _logger.Info("Getting city list csv file path from config file");
                string strFilePath = ConfigurationManager.AppSettings["CityListFile"].ToString();
                _logger.Info("Generating datatable from city list csv file");
                DataTable city_List = GetDataTableFromCsv(strFilePath, true);
                _logger.Info("Generating datatable from city list csv file completed");

                _logger.Info("Getting latitude and longitude data from city list data");
                foreach (DataRow drow in city_List.Rows)
                {
                    if (drow["city_ascii"].ToString().ToLower() == cityName.ToLower())
                    {
                        city.CityName = cityName;
                        city.Latitude = Convert.ToDecimal(drow["lat"].ToString());
                        city.Longtitude = Convert.ToDecimal(drow["lng"].ToString());
                        _logger.Info("Getting latitude and longitude data from city list data completed and match found");
                        break;
                    }
                }
                if (city.CityName == null)
                {
                    _logger.Info("Getting latitude and longitude data from city list data completed but no match found");
                    city.Status = false;
                }
                else
                {
                    city.Status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal("Error Occured while getting lat-long details from data layer");
                _logger.Fatal("Error message:\n" + ex.Message);
                city.CityName = null;
            }
            return city;
        }

        
        //Convert CSV file to DataTable for faster accessing data in it
        static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            _logger.Info("Checking CSV file for header information to assign it as datatable header");
            string header = isFirstRowHeader ? "Yes" : "No";

            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            string sql = @"SELECT * FROM [" + fileName + "]";
            _logger.Info("Opening the csv file and fetching data");
            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                      ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }
}
