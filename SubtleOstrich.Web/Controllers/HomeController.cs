using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SubtleOstrich.Logic;

namespace SubtleOstrich.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Activity()
        {
            return View("Activity");
        }

        public JsonResult ActivityList()
        {
            var u = new User("240747413", "twitter");
            var activities = u.GetActivities(DateTime.Today);
            return Json(activities, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void ActivityList(Occurrence occ)
        {
            if(occ.Date == DateTime.MinValue)
                occ.Date = DateTime.Today;

            var u = new User("240747413", "twitter");
            u.AddRecord(occ.Name, new Record(occ.Date, occ.Note));
        }
    }
}
