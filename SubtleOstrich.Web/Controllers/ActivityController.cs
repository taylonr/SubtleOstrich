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
    }
}
