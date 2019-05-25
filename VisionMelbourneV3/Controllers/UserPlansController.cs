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

namespace VisionMelbourneV3.Controllers
{
    [Authorize]
    public class UserPlansController : Controller
    {
        //variable for plan model. This model is a virtual database for planner tables
        private Plan db = new Plan();

        // GET: UserPlans
        public ActionResult Index()
        {
            //Get the user ID of logged in User
            var currentUserId = User.Identity.GetUserId();
            UserPlan userPlan = new UserPlan();
            //check if the user ID in UserPlans table contains the current logged in User
            var plans = from a in db.UserPlans where a.UserID == currentUserId select a;
            //return a list of plans for the current user
            return View(plans.ToList());
        }

        [AllowAnonymous]
        public double Distance(String lat1, String lon1, String lat2, String lon2)
        {
            //The geodesy library calculates distance between two geo coordinates based on
            //Vincenty's Formula. This library class contains parameters to calculate the distance.
            //The Ellipsoid value is based on the geographic location. Australia follows GRS80 
            //Ellipsoid based on its demography.
            //The class was created by Gavaghan http://www.gavaghan.org/blog/free-source-code/geodesy-library-vincentys-formula/

            Ellipsoid reference = Ellipsoid.GRS80;

            //Create new object for geodetic calculator.
            GeodeticCalculator geoCalc = new GeodeticCalculator(reference);
            
           //Set the coordinates for point A
            GlobalCoordinates pointA;
            pointA = new GlobalCoordinates(
                new Angle(Convert.ToDouble(lat1)), new Angle(Convert.ToDouble(lon1))
            );

            //Set the coordinates for point B
            GlobalCoordinates pointB;
            pointB = new GlobalCoordinates(
                new Angle(Convert.ToDouble(lat2)), new Angle(Convert.ToDouble(lon2))
            );

            //Calculate the curved distance between the two coordinates
            GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(pointA, pointB);
            double ellipseMeters = geoCurve.EllipsoidalDistance;
            return ellipseMeters; // return the distance in meters
        } 

        private string PedestrianCount(string latitude, string longitude, string day, string hr)
        {
            //intialise a counter to calculate the average pedestrain count for the location
            int i = 1;
            //variable to store the people count taken by each sensor at given time
            int peopleCount = 0;
            //Get the list of sensors from the database
            var sensorLocations = db.SensorLocations.ToList();
            //iterate thruogh all the sensor details available in the database 
            foreach (var count in sensorLocations)
            {
                //call the geodesy funciton to calculate the distance between user selected location and sensor location
                double dist = Distance(latitude, longitude, count.latitude, count.longitude);
                //check if the returned distance is less than 170 meters.
                if (dist <= 170)
                {
                    //get the pedestrain count based on day and hour chosen, recorded by the closest sensor
                    var pedcount = from a in db.Pedcounts
                                   where a.SensorID.Equals(count.sensor_id)
                                   && a.Day.Equals(day) && a.Time.Equals(hr)
                                   select a;
                    //iterate through the list of pedestrian count data obtained for specific day and hour
                    foreach (var c in pedcount.ToList())
                    {
                        //sum the counts
                        peopleCount = peopleCount + (int)c.PedCount1;
                        //increment the counter to calculate average
                        i++;
                    }
                }
            }
            return Convert.ToString(peopleCount / i);
        }


        [AllowAnonymous]
        // GET: UserPlans/Details/id
        public ActionResult Details(int? id, DateTime date, string fromlocation)
        {
            DateTime newTime = Convert.ToDateTime(date);
            TimeSpan ntime = newTime.TimeOfDay;
            DateTime nplandate = Convert.ToDateTime(date);
            var start = fromlocation;
            //check if ID requested is null. If null return bad request page
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var time = date;

            //calculate the Day and hour from the time chosen by the user
            var day = Convert.ToString(time.DayOfWeek);
            var hr = Convert.ToString(time.Hour);

            //Find the location matching the id from the database
            Location location = db.Locations.Find(id);
            // get the average pedestrian count for the given location at given day and hour
            string avgCount = PedestrianCount(location.Latitude, location.Longitude, day, hr);
            //create a detailed location object to store in the database to display to the user
            DetailedLocation detailedLocation = new DetailedLocation
            {
                StartLocation = start,
                LocationID = location.Id,
                Location = location.Name,
                Theme = location.Theme,
                Latitude = Convert.ToDouble(location.Latitude),
                Longitude = Convert.ToDouble(location.Longitude),
                PeopleCount = avgCount,
                Date = date,
                Time = ntime,
                AccessibilityLevel = location.AccessibilityLevel,
                AccessibilityRating = location.AccessibilityRating
            };
            //return the newly added location to the view model
            return View(detailedLocation);
        }

        [AllowAnonymous]
        public ActionResult AnyLocationDetails(string location, double lat, double lon, DateTime date)
        {
            DateTime newTime = Convert.ToDateTime(date);
            TimeSpan ntime = newTime.TimeOfDay;
            DateTime nplandate = Convert.ToDateTime(date);
            var time = date;

            //calculate the Day and hour from the time chosen by the user
            var day = Convert.ToString(time.DayOfWeek);
            var hr = Convert.ToString(time.Hour);
            string avgCount = PedestrianCount(Convert.ToString(lat), Convert.ToString(lon), day, hr);
            DetailedLocation detailedLocation = new DetailedLocation
            {
                Location = location,
                Latitude = Convert.ToDouble(lat),
                Longitude = Convert.ToDouble(lon),
                PeopleCount = avgCount,
                Date = date,
                Time = ntime
            };
            return View(detailedLocation);
        }


        public ActionResult PlanCreator(string date)
        {
            //check if the date chosen is null. If yes redirect to index page.
            if (String.IsNullOrEmpty(date))
            {
                return RedirectToAction("Index","Home");
            }
            //get the current logged in User's ID
            var currentUserId = User.Identity.GetUserId();
            DateTime plandate = Convert.ToDateTime(date);
            UserPlan userPlan = new UserPlan();

            //Check the user Id and date chosen by that user. Retrieve the plans from the database.
            var plans = from a in db.UserPlans where a.UserID == currentUserId && a.Date == plandate select a;

            //Sort them based on time to display to the user.
            plans = plans.OrderBy(p => p.Time);
            List<UserPlan> planList = new List<UserPlan>();
            //Add all the plans to a list to display in view model
            foreach (var id in plans)
            {
                planList.Add(userPlan = db.UserPlans.Find(id.Id));
            }
            //If no plans were found, create an object with date attribute set to display the date to the user.
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

            //calculate the Day and hour from the time chosen by the user
            var day = Convert.ToString(nplandate.DayOfWeek);
            var hr = Convert.ToString(ntime.Hours);
            string avgCount = PedestrianCount(lat, lon, day, hr);
            UserPlan newPlan = new UserPlan();
            Location foundLocation = new Location();
            //check if the latitude and longitude values of the location chosen by user match the stored
            //locations in the database.
            var findLocation = from a in db.Locations where a.Name == location && a.Latitude == lat
                     && a.Longitude == lon select a;
            //if any location is found, get the id and obtain the accessibility level for that location
            foreach (var id in findLocation)
            {
               foundLocation = db.Locations.Find(id.Id);
                if (foundLocation != null)
                {
                    newPlan.AccessibilityLevel = foundLocation.AccessibilityLevel;
                    newPlan.AccessibilityRating = foundLocation.AccessibilityRating;
                }
            }
            
            //get the current logged in User's ID
            var currentUserId = User.Identity.GetUserId();
            newPlan.UserID = currentUserId;
            newPlan.StartLocation = fromlocation;
            newPlan.Date = nplandate;
            newPlan.Location = location;
            newPlan.Latitude = Convert.ToDouble(lat);
            newPlan.Longitude = Convert.ToDouble(lon);
            newPlan.Time = ntime;
            newPlan.PeopleCount = avgCount;
            //add useplan to the database
            db.UserPlans.Add(newPlan);
            //save changes made to the database
            db.SaveChanges();
            //return to Plan creator action to display all plans
            return RedirectToAction("PlanCreator", new { date = plandate });
        }
         
        //GET: UserPlans/Nearbyplaces
        public ViewResult NearbyPlaces(string planstart, string nearlocation, string date, string locLat, string locLng, string planTime)
        {
            //Create a Viewbag model to store details about the user selected location
            ViewBag.planstartLocation = planstart;
            ViewBag.findnearLocation = nearlocation;
            ViewBag.planDate = date;
            ViewBag.latitude = locLat;
            ViewBag.longitude = locLng;
            ViewBag.Time = planTime;
            //return a list of locations from the database
            return View(db.Locations.ToList());
        }

        //GET: UserPlans/CategoryIndex
        [AllowAnonymous]
        public ActionResult CategoryIndex(string category)
        {
            //get the locations whose category matches the user selected category
            var locations = db.Locations.Where(g => g.Theme.Contains(category)).ToList();
            //return the list of locations retrieved 
            return View(locations);
        }


        // GET: UserPlans/Delete/5
        public ActionResult Delete(int? id)
        {
            //check if ID is null.
            if (id == null)
            {
                //return bad request page
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find the plan ID to delete.
            UserPlan userPlan = db.UserPlans.Find(id);
            //Return not found is the Object is empty
            if (userPlan == null)
            {
                return HttpNotFound();
            }
            //return a view with the details of the user plan to be deleted
            return View(userPlan);
        }


        // POST: UserPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //On confirm find the Userplan
            UserPlan userPlan = db.UserPlans.Find(id);
            //get the plandate that the user was viewing to redirect to the same page
            DateTime plandate = userPlan.Date;
            //remove plan and save changes to database
            db.UserPlans.Remove(userPlan);
            db.SaveChanges();
            return RedirectToAction("PlanCreator", new { date = plandate });
        }

        //Dispose the garbage values and variables that are unused
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
