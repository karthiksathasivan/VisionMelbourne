using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using VisionMelbourneV3.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;



namespace VisionMelbourneV3.Controllers
{
   
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LearnBraille()
        {
            ViewBag.Message = "Learn Braille";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Accessibility Tips";

            return View();
        }
        
        public ActionResult TactileSurface()
        {
            ViewBag.Message = "Tactile Surfaces";

            return View();
        }

        //Constructing Weather object which contains the API call to OpenWeather 
        public JsonResult GetWeather()
        {
            Weather weather = new Weather();
            return Json(weather.getWeatherForecast(), JsonRequestBehavior.AllowGet);
        }
    }
}