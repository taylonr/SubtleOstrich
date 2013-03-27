using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SubtleOstrich.Logic;

namespace SubtleOstrich.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Activity()
        {
            return View("Activity");
        }

        public JsonResult ActivityList(DateTime? date)
        {
            Console.WriteLine(User.Identity.Name);
            var u = new User("240747413", "twitter");
            var activities = u.GetActivities(date ?? DateTime.Today);
            return Json(activities, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityList(Occurrence occ)
        {
            if (occ.Date == DateTime.MinValue)
                occ.Date = DateTime.Today;

            var u = new User("240747413", "twitter");
            occ.Id = u.AddRecord(occ.Name, new Record(occ.Date, occ.Note));
            return Json(occ, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public void ActivityList(string id)
        {
            var u = new User("240747413", "twitter");
            u.DeleteRecord(id);
        }

        [HttpGet]
        public JsonResult MonthDashboard()
        {
            var u = new User("240747413", "twitter");
            return Json(u.GetMonthDashboard(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult YearDashboard()
        {
            var u = new User("240747413", "twitter");
            return Json(u.GetYearDashboard(), JsonRequestBehavior.AllowGet);            
        }
}
}
