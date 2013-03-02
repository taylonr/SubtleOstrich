using System;
using System.Collections.Generic;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Authentication.WorldDomination;
using Nancy.Security;
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
            
            return nancyModule.Negotiate.WithView("Home").WithModel(u);
        }
    }

    public class UserMapper : IUserMapper
    {
        private readonly IUserRepository _repository;

        public UserMapper()
        {
            _repository = new UserRepository();
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var u = _repository.GetByGuid(identifier);
            if (u == null)
                return null;

            return new UserIdentity {UserName = u.Name};
        }
    }

    public class UserIdentity : IUserIdentity
    {
        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}