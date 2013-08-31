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
        [Authorize]
        public ActionResult Index()
        {
            var u = new User(User.Uid, User.Source);
            var activity = u.Activities.Select(a => new Occurrence { Name = a.Name, Hours = a.Hours });
            return View(activity);
        }

        
        [HttpPost]
        [Authorize]
        public ActionResult Activities(IEnumerable<Occurrence> occurences)
        {
            var u = new User(User.Uid, User.Source);
            foreach(var occ in occurences)
            {
                var activity = u.Activities.FirstOrDefault(a => a.Name == occ.Name);
                activity.Hours = occ.Hours ?? 0;
                u.Save();
            }

            return RedirectToAction("Index", "Activity");
        }

    }
}
