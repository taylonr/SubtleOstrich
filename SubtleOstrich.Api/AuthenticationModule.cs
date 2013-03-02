using Nancy;

namespace SubtleOstrich.Api
{
    public class AuthenticationModule : NancyModule
    {
        public AuthenticationModule()
            : base("/authentication")
        {
            Get["/redirect"] = Redirect;
            Get[""] = _ => View["login.html"];
            Get["/authenticatecallback"] = parameters =>
                {
                    return HttpStatusCode.OK;
                };

        }

        private Response Redirect(dynamic o)
        {
            return HttpStatusCode.OK;
        }
    }
}