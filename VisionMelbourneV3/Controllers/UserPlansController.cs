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
        public ViewResult Index(string radius, string date)
        {
            List<UserPlan> detailedLocations = new List<UserPlan>();
            DateTime time = DateTime.Now;
            var day = Convert.ToString(time.DayOfWeek);
            var hr = Convert.ToString(time.Hour);
            detailedLocations.AddRange(db.UserPlans.ToList());
            db.UserPlans.RemoveRange(detailedLocations);
            db.SaveChanges();
            detailedLocations.Clear();
            if (String.IsNullOrEmpty(radius))
            {
                radius = "150";
            }
            if (!String.IsNullOrEmpty(date))
            {
                DateTime d = Convert.ToDateTime(date);
                day = Convert.ToString(d.DayOfWeek);
                hr = Convert.ToString(d.Hour);                
            }
           
            var locations = db.Locations.ToList();            
            foreach(var item in locations)
            {
                var latitude = item.Latitude;
                var longitude = item.Longitude;
                int i = 1;
                int peopleCount = 0;
                
                var sensorLocations = db.SensorLocations.ToList();
                foreach (var count in sensorLocations)
                {
                    double dist = distance(latitude, longitude, count.latitude, count.longitude);               
                    if(dist <= Convert.ToDouble(radius)) {                                                                    
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
            System.Diagnostics.Debug.WriteLine(detailedLocations.Count);
            return View(db.UserPlans.ToList());
        }

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
            UserPlan userPlan = db.UserPlans.Find(id);
            if (userPlan == null)
            {
                return HttpNotFound();
            }
            return View(userPlan);
        }

        public ActionResult CategoryIndex(string category, string radius, string date)
        {
            List<UserPlan> detailedLocations = new List<UserPlan>();
            DateTime time = DateTime.Now;
            var day = Convert.ToString(time.DayOfWeek);
            var hr = Convert.ToString(time.Hour);
            detailedLocations.AddRange(db.UserPlans.ToList());
            db.UserPlans.RemoveRange(detailedLocations);
            db.SaveChanges();
            detailedLocations.Clear();
            if (String.IsNullOrEmpty(radius))
            {
                radius = "150";
            }
            if (!String.IsNullOrEmpty(date))
            {
                DateTime d = Convert.ToDateTime(date);
                day = Convert.ToString(d.DayOfWeek);
                hr = Convert.ToString(d.Hour);
            }

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
        // GET: UserPlans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Location,Latitude,Longitude,PeopleCount,Tactile,Weather,UserID")] UserPlan userPlan)
        {
            if (ModelState.IsValid)
            {
                db.UserPlans.Add(userPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userPlan);
        }

        // GET: UserPlans/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: UserPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Location,Latitude,Longitude,PeopleCount,Tactile,Weather,UserID")] UserPlan userPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userPlan);
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
