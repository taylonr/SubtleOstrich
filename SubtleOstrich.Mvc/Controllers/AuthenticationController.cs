using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SubtleOstrich.Logic;
using WorldDomination.Web.Authentication.Mvc;

namespace SubtleOstrich.Mvc.Controllers
{
    public class AuthenticationController : Controller, IAuthenticationCallbackProvider
    {
        private readonly IUserRepository _repository;

        public AuthenticationController()
            : this(new UserRepository())
        {
        }

        public AuthenticationController(IUserRepository repo)
        {
            _repository = repo;
        }

        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model)
        {
            var u = _repository.GetBySourceAndId(model.AuthenticatedClient.ProviderName, model.AuthenticatedClient.UserInformation.Id);

            if (u == null)
            {
                u = new User(model.AuthenticatedClient.UserInformation.Id, model.AuthenticatedClient.UserInformation.Name, model.AuthenticatedClient.ProviderName);
                u.Save();
            }
            var authTicket = new FormsAuthenticationTicket(u.Name, true, 60);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
            context.Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Root");
        }
    }
}