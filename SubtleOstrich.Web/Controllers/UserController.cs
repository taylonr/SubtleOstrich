using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SubtleOstrich.Logic;

namespace SubtleOstrich.Web.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult Activities()
        {
            var u = new User(User.Uid, User.Source);
            var activity = u.Activities.Select(a => new Occurrence{Name = a.Name, Hours = a.Hours});
            return Json(activity, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Activities(IEnumerable<Occurrence> a)
        {
            return Json(a);
        }

    }
}
