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
using Microsoft.AspNet.Identity;


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
            ViewBag.Message = "Talkbacks Tips";

            return View();
        }

        public ActionResult SendSms(String userLat, String userLon)
        {
            //get the user ID of the current logged in user
            var currentUser = User.Identity.GetUserId();
            //get an instance of the User DB
            ApplicationDbContext db = new ApplicationDbContext();
            //Find the user name and emergency contact numbers for the current user
            var userResult = db.Users.FirstOrDefault(u => u.Id == currentUser);
            var userName = userResult.UserName;
            var emergencyPhone = userResult.EmergencyContact;
            //Initialise the twilio client object
            var accountSid = "{YOUR_ID}";
            var authToken = "{YOUR_TOKEN}";
            TwilioClient.Init(accountSid, authToken);
            var to = new PhoneNumber(emergencyPhone);
            var from = new PhoneNumber("+61429594296");
            var message = MessageResource.Create(
                to: to,
                from: from,
                body: "EMERGENCY! " +
                "Your friend "+ userName +" is in an emergency now! Plseae click the link to get the location  - https://www.google.com/maps/search/?api=1&query=" + userLat + "," + userLon);
            return Content(message.Sid);
        }

        public JsonResult GetWeather()
        {
            Weather weather = new Weather();
            return Json(weather.getWeatherForecast(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupportServices()
        {
            ViewBag.Message = "Support Services ";

            return View();
        }


        public ActionResult TactileSurface()
        {
            ViewBag.Message = "TactileSurface Services ";

            return View();
        }

    }
}
