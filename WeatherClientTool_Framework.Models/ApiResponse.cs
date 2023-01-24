using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherClientTool_Framework.Models
{
    public class ApiResponse
    {
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }

        public decimal generationtime_ms { get; set; }

        public int utc_offset_seconds { get; set; }

        public string timezone { get; set;  }
        public string timezone_abbreviation { get; set; }
        public decimal elevation { get; set; }

        public Current_weather current_Weather { get; set; }
    }
    public class Current_weather
    {
        public decimal temperature { get; set; }
        public decimal windspeed { get; set; }
        public decimal winddirection { get; set; }
        public int weathercode { get; set; }

        public DateTime time { get; set; }

    }
}
