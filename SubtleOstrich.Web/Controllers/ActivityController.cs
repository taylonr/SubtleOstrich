﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SubtleOstrich.Logic;

namespace SubtleOstrich.Web.Controllers
{
    public class ActivityController : BaseController
    {
        //
        // GET: /Activity/

        public ActionResult Index()
        {
            if (User == null || !User.Identity.IsAuthenticated)
                return View("LandingPage");

            return View("Activity");
        }

        [Authorize]
        public JsonResult ActivityList(DateTime? date)
        {
            var u = new User(User.Uid, User.Source);
            var activities = u.GetActivities(date ?? DateTime.Today);
            return Json(activities, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoComplete(string term)
        {
            var u = new User(User.Uid, User.Source);
            var activities = u.Activities.Where(x => x.Name.ToLower().Contains(term.ToLower())).Select(x => x.Name).Distinct();
            return Json(activities, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult ActivityList(Occurrence occ)
        {
            if (occ.Date == DateTime.MinValue)
                occ.Date = DateTime.Today;

            var u = new User(User.Uid, User.Source);
            var nameIndex = occ.Name.IndexOfAny(new[] {':', '!'});
            var name = nameIndex == -1 ? occ.Name : occ.Name.Substring(0, nameIndex);

            var text = occ.Name.Split(':');
            var hours = occ.Name.Split('!');

            occ.Name = name.Trim();

            if (text.Length == 2)
                occ.Note = text[1].Trim();

            
            if (hours.Length == 2)
            {
                decimal time;
                decimal.TryParse(hours[1], out time);
                occ.Hours = time;
            }

            occ.Id = u.AddRecord(occ.Name, new Record(occ.Date, occ.Note, occ.Hours));
            return Json(occ);
        }

        [HttpDelete]
        [Authorize]
        public void ActivityList(string id)
        {
            var u = new User(User.Uid, User.Source);
            u.DeleteRecord(id);
        }

        [HttpGet]
        [Authorize]
        public JsonResult MonthDashboard()
        {
            var u = new User(User.Uid, User.Source);
            return Json(u.GetMonthDashboard(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public JsonResult YearDashboard()
        {
            var u = new User(User.Uid, User.Source);
            return Json(u.GetYearDashboard(), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Calendar()
        {
            return View();
        }

        [Authorize]
        public JsonResult CalendarEvents(double start, double end)
        {
            var u = new User(User.Uid, User.Source);
            var activities = u.GetCalendarEvents(start.ToDateTime(), end.ToDateTime());

            return Json(activities, JsonRequestBehavior.AllowGet);
        }

        public JsonResult YearReport(string source, string uid)
        {
            var u = new User(uid,source);
            var activities = u.GetYearReport(DateTime.Now.Year);

            return Json(activities, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MonthReport(string source, string uid)
        {
            var u = new User(uid, source);
            var activities = u.GetMonthReport(DateTime.Now);

            return Json(activities, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Report(string uid, string source)
        {
            return View("Report", new User(uid, source));
        }

        public JsonResult ReportEvents(string id, double start, double end)
        {
            var user = id.Split(':');
            var u = new User(user[1], user[0]);
            var year = DateTime.Now.Year;
            var activities = u.GetCalendarEvents(new DateTime(year, 1, 1), new DateTime(year, 12, 31));

            return Json(activities, JsonRequestBehavior.AllowGet);
        }
    }

}
