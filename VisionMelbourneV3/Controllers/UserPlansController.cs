using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VisionMelbourneV3.Models;

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
        {  // generally used geo measurement function

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

        // GET: UserPlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailedLocation userPlan = db.DetailedLocations.Find(id);
            if (userPlan == null)
            {
                return HttpNotFound();
            }
            return View(userPlan);
        }



        public ActionResult PlanCreator(DateTime date)
        {
            UserPlan userPlan = new UserPlan();
            var plans = from a in db.UserPlans where a.Date == date select a;
            plans = plans.OrderBy(p => p.Time);
            List<UserPlan> planList = new List<UserPlan>();
            foreach (var id in plans)
            {
                planList.Add(userPlan = db.UserPlans.Find(id.Id));               
            }
            if (planList.Count == 0)
            {
                userPlan.Date = date;
                planList.Add(userPlan);
            }            
            return View("PlanCreator", planList);
        }


        public ActionResult NewPlan(string location, Double lat, Double lon, DateTime plandate, TimeSpan time)
        {
            var day = Convert.ToString(plandate.DayOfWeek);
            var hr = Convert.ToString(time.Hours);
            int i = 1;
            int peopleCount = 0;

            var sensorLocations = db.SensorLocations.ToList();
            foreach (var count in sensorLocations)
            {
                double dist = distance(Convert.ToString(lat), Convert.ToString(lon), count.latitude, count.longitude);
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
            newPlan.Date = plandate;
            newPlan.Location = location;
            newPlan.Latitude = lat;
            newPlan.Longitude = lon;
            newPlan.Time = time;
            newPlan.PeopleCount = avgCount;
            db.UserPlans.Add(newPlan);
            db.SaveChanges();

            return RedirectToAction("PlanCreator", new { date = plandate });
        }


        public ActionResult CategoryIndex(string category, string radius, string date)
        {
            List<DetailedLocation> detailedLocations = new List<DetailedLocation>();
            DateTime time = DateTime.Now;
            detailedLocations.AddRange(db.DetailedLocations.ToList());
            db.DetailedLocations.RemoveRange(detailedLocations);
            db.SaveChanges();
            detailedLocations.Clear();
            if (String.IsNullOrEmpty(radius))
            {
                radius = "150";
            }
            if (!String.IsNullOrEmpty(date))
            {
                time = Convert.ToDateTime(date);
            }
            var day = Convert.ToString(time.DayOfWeek);
            var hr = Convert.ToString(time.Hour);
            var locations = db.Locations.Where(g => g.Theme == category).ToList();
            foreach (var item in locations)
            {
                var latitude = item.Latitude;
                var longitude = item.Longitude;
                int i = 1;
                int peopleCount = 0;

                var sensorLocations = db.SensorLocations.ToList();
                foreach (var count in sensorLocations)
                {
                    double dist = distance(latitude, longitude, count.latitude, count.longitude);
                    if (dist <= Convert.ToDouble(radius))
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
                UserPlan userPlan = new UserPlan();
                userPlan.Location = item.Name;
                userPlan.Latitude = Convert.ToDouble(item.Latitude);
                userPlan.Longitude = Convert.ToDouble(item.Longitude);
                userPlan.PeopleCount = avgCount;
                userPlan.Date = time;
                db.UserPlans.Add(userPlan);
                db.SaveChanges();
            }
            return View(db.UserPlans.ToList());
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
            db.UserPlans.Remove(userPlan);
            db.SaveChanges();
            return RedirectToAction("Index");
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
