using System.Web.Mvc;

namespace SubtleOstrich.Web.Controllers
{
    public class BaseController : Controller
    {
        protected virtual new CouchPrincipal User
        {
            get { return HttpContext.User as CouchPrincipal; }
        }
    }
}