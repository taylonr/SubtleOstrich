using System;
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
            var text = occ.Name.Split(':');

            occ.Name = text[0].Trim();

            if (text.Length == 2)
                occ.Note = text[1].Trim();

            occ.Id = u.AddRecord(occ.Name, new Record(occ.Date, occ.Note));
            return Json(occ, JsonRequestBehavior.AllowGet);
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
