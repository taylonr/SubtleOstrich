using Nancy;
using Nancy.Authentication.WorldDomination;

namespace SubtleOstrich.Api
{
    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            return nancyModule.Negotiate.WithView("AuthenticateCallback").WithModel(model);
        }
    }
}