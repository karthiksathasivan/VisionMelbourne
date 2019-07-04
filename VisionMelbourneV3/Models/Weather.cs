using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace VisionMelbourneV3.Models
{
    public class Weather
    {
        public Object getWeatherForecast()
        {
            string url = "http://api.openweathermap.org/data/2.5/forecast?lat=-37.813061&lon=144.944214&APPID={YOUR_ID}&units=metric";
            var client = new WebClient();
            var content = client.DownloadString(url);
            var serializer = new JavaScriptSerializer();
            var jsonContent = serializer.Deserialize<Object>(content);
            return jsonContent;
        }
    }
}
