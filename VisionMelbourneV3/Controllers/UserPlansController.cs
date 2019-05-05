using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VisionMelbourneV3.Models;
using Microsoft.AspNet.Identity;

namespace VisionMelbourneV3.Controllers
{
    public class UserPlansController : Controller
    {

        private Plan db = new Plan();
        // GET: UserPlans
        //public ActionResult Index(string date)
        //{
        //    List<DetailedLocation> detailedLocations = new List<DetailedLocation>();
        //    detailedLocations.AddRange(db.DetailedLocations.ToList());
        //    db.DetailedLocations.RemoveRange(detailedLocations);
        //    db.SaveChanges();
        //    detailedLocations.Clear();
        //    DateTime time = DateTime.Now;
        //    if (!String.IsNullOrEmpty(fromLocation))
        //    {               
        //        if (!String.IsNullOrEmpty(date))
        //        {
        //            time = Convert.ToDateTime(date);
        //        }
        //        var day = Convert.ToString(time.DayOfWeek);
        //        var hr = Convert.ToString(time.Hour);
        //        var locations = db.Locations.ToList();
        //        foreach (var item in locations)
        //        {
        //            var latitude = item.Latitude;
        //            var longitude = item.Longitude;
        //            int i = 1;
        //            int peopleCount = 0;

        //            var sensorLocations = db.SensorLocations.ToList();
        //            foreach (var count in sensorLocations)
        //            {
        //                double dist = distance(latitude, longitude, count.latitude, count.longitude);
        //                if (dist <= 150)
        //                {
        //                    var pedcount = from a in db.Pedcounts
        //                                   where a.SensorID.Contains(count.sensor_id)
        //                                   && a.Day.Contains(day) && a.Time.Contains(hr)
        //                                   select a;
        //                    foreach (var c in pedcount.ToList())
        //                    {
        //                        peopleCount = peopleCount + (int)c.PedCount1;
        //                        i++;
        //                    }
        //                }
        //            }
        //            string avgCount = Convert.ToString(peopleCount / i);
        //            DetailedLocation detailedLocation = new DetailedLocation();
        //            detailedLocation.Location = item.Name;
        //            detailedLocation.Latitude = Convert.ToDouble(item.Latitude);
        //            detailedLocation.Longitude = Convert.ToDouble(item.Longitude);
        //            detailedLocation.PeopleCount = avgCount;
        //            detailedLocation.Date = time;
        //            detailedLocation.Theme = item.Theme;
        //            db.DetailedLocations.Add(detailedLocation);
        //            db.SaveChanges();
        //        }
        //        return View(db.DetailedLocations.ToList());
        //    }
        //    if (!String.IsNullOrEmpty(date))
        //    {
        //        time = Convert.ToDateTime(date);
        //    }
        //    return RedirectToAction("PlanCreator", new { startdate = time });
        //}


        public double distance(String lat1, String lon1, String lat2, String lon2)
        {  
            // generally used geo measurement function
            var R = 6378.137; // Radius of earth in KM
            var dLat = Convert.ToDouble(lat2) * Math.PI / 180 - Convert.ToDouble(lat1) * Math.PI / 180;
            var dLon = Convert.ToDouble(lon2) * Math.PI / 180 - Convert.ToDouble(lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(Convert.ToDouble(lat1) * Math.PI / 180) * Math.Cos(Convert.ToDouble(lat2) * Math.PI / 180) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d * 1000; // meters
        }

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
            string dayPeopleCount = "";           
            Location location = db.Locations.Find(id);

            var latitude = location.Latitude;
            var longitude = location.Longitude;
            int i = 1;
            int peopleCount = 0;

            var sensorLocations = db.SensorLocations.ToList();
            foreach (var count in sensorLocations)
            {
                double dist = distance(latitude, longitude, count.latitude, count.longitude);
                if (dist <= Convert.ToDouble(150))
                {
                    var pedcount = from a in db.Pedcounts
                                   where a.SensorID.Contains(count.sensor_id)
                                   && a.Day.Contains(day) && a.Time.Contains(hr)
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



        public ActionResult PlanCreator(string date)
        {
            var currentUserId = User.Identity.GetUserId();
            DateTime plandate = Convert.ToDateTime(date);
            UserPlan userPlan = new UserPlan();
            //var plans = from a in db.UserPlans where a.Date == plandate && a.StartLocation == startlocation select a;
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
                //userPlan.StartLocation = startlocation;
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
                if (dist <= 150)
                {
                    var pedcount = from a in db.Pedcounts
                                   where a.SensorID.Contains(count.sensor_id)
                                   && a.Day.Contains(day) && a.Time.Contains(hr)
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

            return RedirectToAction("PlanCreator", new { date = plandate});
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

        //[HttpPost]
        //public ActionResult EditedPlan(UserPlan userPlan)
        //{
        //    var day = Convert.ToString(userPlan.Date.DayOfWeek);
        //    TimeSpan time = (TimeSpan)userPlan.Time;
        //    var hr = Convert.ToString(time.Hours);
        //    int i = 1;
        //    int peopleCount = 0;

        //    var sensorLocations = db.SensorLocations.ToList();
        //    foreach (var count in sensorLocations)
        //    {
        //        double dist = distance(Convert.ToString(userPlan.Latitude), Convert.ToString(userPlan.Longitude), count.latitude, count.longitude);
        //        if (dist <= 150)
        //        {
        //            var pedcount = from a in db.Pedcounts
        //                           where a.SensorID.Contains(count.sensor_id)
        //                           && a.Day.Contains(day) && a.Time.Contains(hr)
        //                           select a;
        //            foreach (var c in pedcount.ToList())
        //            {
        //                peopleCount = peopleCount + (int)c.PedCount1;
        //                i++;
        //            }
        //        }
        //    }
        //    string avgCount = Convert.ToString(peopleCount / i);

        //    UserPlan newPlan = db.UserPlans.SingleOrDefault(x => x.Id == userPlan.Id);
        //    newPlan.Date = userPlan.Date;
        //    newPlan.Location = userPlan.Location;
        //    newPlan.Latitude = userPlan.Latitude;
        //    newPlan.Longitude = userPlan.Longitude;
        //    newPlan.Time = userPlan.Time;
        //    newPlan.PeopleCount = avgCount;
        //    db.SaveChanges();
        //    return RedirectToAction("PlanCreator", new { date = userPlan.Date });
        //}


        //public ActionResult EditPlan(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserPlan userPlan = db.UserPlans.Find(id);
        //    if (userPlan == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return PartialView("EditPlan", userPlan);
        //}

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
            return RedirectToAction("PlanCreator", new { date = plandate});
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
