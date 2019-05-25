using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Geodesy;
using VisionMelbourneV3.Models;
using Microsoft.AspNet.Identity;

//Controller to manage all 
namespace VisionMelbourneV3.Controllers
{
    [Authorize]
    public class UserPlansController : Controller
    {

        private Plan db = new Plan();
        // GET: UserPlans
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            UserPlan userPlan = new UserPlan();
            var plans = from a in db.UserPlans where a.UserID == currentUserId select a;
            return View(plans.ToList());
        }


        [AllowAnonymous]
        public double distance(String lat1, String lon1, String lat2, String lon2)
        {
            
            Ellipsoid reference = Ellipsoid.GRS80;
            GeodeticCalculator geoCalc = new GeodeticCalculator(reference);
            
            GlobalCoordinates lincolnMemorial;
            lincolnMemorial = new GlobalCoordinates(
                new Angle(Convert.ToDouble(lat1)), new Angle(Convert.ToDouble(lon1))
            );

            GlobalCoordinates eiffelTower;
            eiffelTower = new GlobalCoordinates(
                new Angle(Convert.ToDouble(lat2)), new Angle(Convert.ToDouble(lon2))
            );

            GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(lincolnMemorial, eiffelTower);
            double ellipseMeters = geoCurve.EllipsoidalDistance;
            return ellipseMeters;
        }

        [AllowAnonymous]
        // GET: UserPlans/Details/id
        public ActionResult Details(int? id, DateTime date, string fromlocation)
        {
            DateTime newTime = Convert.ToDateTime(date);
            TimeSpan ntime = newTime.TimeOfDay;
            DateTime nplandate = Convert.ToDateTime(date);
            var start = fromlocation;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var time = date;
            var day = Convert.ToString(time.DayOfWeek);
            var hr = Convert.ToString(time.Hour);
            Location location = db.Locations.Find(id);

            var latitude = location.Latitude;
            var longitude = location.Longitude;
            int i = 1;
            int peopleCount = 0;

            var sensorLocations = db.SensorLocations.ToList();
            foreach (var count in sensorLocations)
            {
                double dist = distance(latitude, longitude, count.latitude, count.longitude);
                if (dist <= 170)
                {
                    var pedcount = from a in db.Pedcounts
                                   where a.SensorID.Equals(count.sensor_id)
                                   && a.Day.Equals(day) && a.Time.Equals(hr)
                                   select a;
                    foreach (var c in pedcount.ToList())
                    {
                        peopleCount = peopleCount + (int)c.PedCount1;
                        i++;
                    }
                }
            }
            string avgCount = Convert.ToString(peopleCount / i);
            DetailedLocation detailedLocation = new DetailedLocation();
            detailedLocation.StartLocation = start;
            detailedLocation.LocationID = location.Id;
            detailedLocation.Location = location.Name;
            detailedLocation.Theme = location.Theme;
            detailedLocation.Latitude = Convert.ToDouble(location.Latitude);
            detailedLocation.Longitude = Convert.ToDouble(location.Longitude);
            detailedLocation.PeopleCount = avgCount;
            detailedLocation.Date = date;
            detailedLocation.Time = ntime;
            detailedLocation.Radius = "150";
            detailedLocation.AccessibilityLevel = location.AccessibilityLevel;
            detailedLocation.AccessibilityRating = location.AccessibilityRating;
            db.DetailedLocations.Add(detailedLocation);
            db.SaveChanges();
            return View(detailedLocation);
        }

        [AllowAnonymous]
        public ActionResult AnyLocationDetails(string location, double lat, double lon, DateTime date)
        {
            DateTime newTime = Convert.ToDateTime(date);
            TimeSpan ntime = newTime.TimeOfDay;
            DateTime nplandate = Convert.ToDateTime(date);
            var time = date;
            var day = Convert.ToString(time.DayOfWeek);
            var hr = Convert.ToString(time.Hour);
            int i = 1;
            int peopleCount = 0;
            var sensorLocations = db.SensorLocations.ToList();
            foreach (var count in sensorLocations)
            {
                double dist = distance(Convert.ToString(lat), Convert.ToString(lon), count.latitude, count.longitude);
                if (dist <= 170)
                {
                    var pedcount = from a in db.Pedcounts
                                   where a.SensorID.Equals(count.sensor_id)
                                   && a.Day.Equals(day) && a.Time.Equals(hr)
                                   select a;
                    
                    foreach (var c in pedcount.ToList())
                    {
                        peopleCount = peopleCount + (int)c.PedCount1;
                        i++;
                    }
                }
            }
            string avgCount = Convert.ToString(peopleCount / i);
            DetailedLocation detailedLocation = new DetailedLocation();
            detailedLocation.Location = location;
            detailedLocation.Latitude = Convert.ToDouble(lat);
            detailedLocation.Longitude = Convert.ToDouble(lon);
            detailedLocation.PeopleCount = avgCount;
            detailedLocation.Date = date;
            detailedLocation.Time = ntime;
            detailedLocation.Radius = "150";
            db.DetailedLocations.Add(detailedLocation);
            db.SaveChanges();
            return View(detailedLocation);
        }


        public ActionResult PlanCreator(string date)
        {
            if (String.IsNullOrEmpty(date))
            {
                return RedirectToAction("Index","Home");
            }
            var currentUserId = User.Identity.GetUserId();
            DateTime plandate = Convert.ToDateTime(date);
            UserPlan userPlan = new UserPlan();
            var plans = from a in db.UserPlans where a.UserID == currentUserId && a.Date == plandate select a;
            plans = plans.OrderBy(p => p.Time);
            List<UserPlan> planList = new List<UserPlan>();
            foreach (var id in plans)
            {
                planList.Add(userPlan = db.UserPlans.Find(id.Id));
            }
            if (planList.Count == 0)
            {
                userPlan.Date = plandate;
                planList.Add(userPlan);
            }
            return View("PlanCreator", planList);
        }


        public ActionResult NewPlan(string fromlocation, string location, string lat, string lon, string plandate, string time)
        {
            DateTime newTime = Convert.ToDateTime(time);
            TimeSpan ntime = newTime.TimeOfDay;
            DateTime nplandate = Convert.ToDateTime(plandate);
            var day = Convert.ToString(nplandate.DayOfWeek);
            var hr = Convert.ToString(ntime.Hours);
            int i = 1;
            int peopleCount = 0;

            var sensorLocations = db.SensorLocations.ToList();
            foreach (var count in sensorLocations)
            {
                double dist = distance(lat, lon, count.latitude, count.longitude);
                if (dist <= 170)
                {
                    var pedcount = from a in db.Pedcounts
                                   where a.SensorID.Equals(count.sensor_id)
                                   && a.Day.Equals(day) && a.Time.Equals(hr)
                                   select a;
                    foreach (var c in pedcount.ToList())
                    {
                        peopleCount = peopleCount + (int)c.PedCount1;
                        i++;
                    }
                }
            }

            string avgCount = Convert.ToString(peopleCount / i);
            UserPlan newPlan = new UserPlan();
            Location foundLocation = new Location();
            var findLocation = from a in db.Locations where a.Name == location && a.Latitude == lat
                     && a.Longitude == lon select a;
            foreach (var id in findLocation)
            {
               foundLocation = db.Locations.Find(id.Id);
            }
            if (foundLocation != null)
            {                
                newPlan.AccessibilityLevel = foundLocation.AccessibilityLevel;
                newPlan.AccessibilityRating = foundLocation.AccessibilityRating;
            }
            var currentUserId = User.Identity.GetUserId();
            newPlan.UserID = currentUserId;
            newPlan.StartLocation = fromlocation;
            newPlan.Date = nplandate;
            newPlan.Location = location;
            newPlan.Latitude = Convert.ToDouble(lat);
            newPlan.Longitude = Convert.ToDouble(lon);
            newPlan.Time = ntime;
            newPlan.PeopleCount = avgCount;
            db.UserPlans.Add(newPlan);
            db.SaveChanges();

            return RedirectToAction("PlanCreator", new { date = plandate });
        }

        public ViewResult NearbyPlaces(string planstart, string nearlocation, string date, string locLat, string locLng, string planTime)
        {
            ViewBag.planstartLocation = planstart;
            ViewBag.findnearLocation = nearlocation;
            ViewBag.planDate = date;
            ViewBag.latitude = locLat;
            ViewBag.longitude = locLng;
            ViewBag.Time = planTime;
            return View(db.Locations.ToList());
        }

        
        [AllowAnonymous]
        public ActionResult CategoryIndex(string category)
        {
            var locations = db.Locations.Where(g => g.Theme.Contains(category)).ToList();

            return View(locations);
        }


        // GET: UserPlans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPlan userPlan = db.UserPlans.Find(id);
            if (userPlan == null)
            {
                return HttpNotFound();
            }
            return View(userPlan);
        }


        // POST: UserPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserPlan userPlan = db.UserPlans.Find(id);
            String fromLocation = userPlan.StartLocation;
            DateTime plandate = userPlan.Date;
            db.UserPlans.Remove(userPlan);
            db.SaveChanges();
            return RedirectToAction("PlanCreator", new { date = plandate });
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
