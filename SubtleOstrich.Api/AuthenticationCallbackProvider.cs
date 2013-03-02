using Nancy;
using Nancy.Authentication.WorldDomination;
using SubtleOstrich.Logic;

namespace SubtleOstrich.Api
{
    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        private IUserRepository _repository;

        public AuthenticationCallbackProvider()
            :this(new UserRepository())
        {
        }

        public AuthenticationCallbackProvider(IUserRepository repo)
        {
            _repository = repo;
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            var u = _repository.GetBySourceAndId(model.AuthenticatedClient.ProviderName, model.AuthenticatedClient.UserInformation.Id);

            if(u == null)
            {
                u = new User(model.AuthenticatedClient.UserInformation.Id, model.AuthenticatedClient.UserInformation.Name, model.AuthenticatedClient.ProviderName);
                u.Save();
            }

            //nancyModule.Request.Session["user"] = u;
            
            return nancyModule.Negotiate.WithView("Home").WithModel(u);
        }
    }
}