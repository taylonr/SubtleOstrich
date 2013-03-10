using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SubtleOstrich.Logic;

namespace SubtleOstrich.Mvc.Controllers
{
    public class UsersController : Controller
    {
        public JsonResult Get(string id)
        {
            return Json(new UserRepository().GetById(id), JsonRequestBehavior.AllowGet);
        }

    }
}
