using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SubtleOstrich.Logic;

namespace SubtleOstrich.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Activity()
        {
            return View("Activity");
        }

        public JsonResult ActivityList(DateTime? date)
        {
            var u = new User(User.Uid, User.Source);
            var activities = u.GetActivities(date ?? DateTime.Today);
            return Json(activities, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityList(Occurrence occ)
        {
            if (occ.Date == DateTime.MinValue)
                occ.Date = DateTime.Today;

            var u = new User(User.Uid, User.Source);
            occ.Id = u.AddRecord(occ.Name, new Record(occ.Date, occ.Note));
            return Json(occ, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public void ActivityList(string id)
        {
            var u = new User(User.Uid, User.Source);
            u.DeleteRecord(id);
        }

        [HttpGet]
        public JsonResult MonthDashboard()
        {
            var u = new User(User.Uid, User.Source);
            return Json(u.GetMonthDashboard(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult YearDashboard()
        {
            var u = new User(User.Uid, User.Source);
            return Json(u.GetYearDashboard(), JsonRequestBehavior.AllowGet);            
        }
}
}
