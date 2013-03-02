using Nancy;
using Nancy.Authentication.Forms;

public class LoginModule : NancyModule
{
    public LoginModule()
    {
        Get["/login"] = parameters => View["login.html"];

        Get["/logout"] = parameters => this.LogoutAndRedirect("/autentication");

        Post["/login"] = parameters => { return HttpStatusCode.OK; };
    }
}