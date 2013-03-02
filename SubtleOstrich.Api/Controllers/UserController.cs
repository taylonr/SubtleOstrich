using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SubtleOstrich.Logic;

namespace SubtleOstrich.Controllers
{
    public class UserController : ApiController
    {
        public void Post(User u)
        {
            u.Save();
        }
    }
}
