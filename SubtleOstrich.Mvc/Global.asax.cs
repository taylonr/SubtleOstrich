using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using SubtleOstrich.Logic;
using WorldDomination.Web.Authentication;
using WorldDomination.Web.Authentication.Facebook;
using WorldDomination.Web.Authentication.Google;
using WorldDomination.Web.Authentication.Mvc;
using WorldDomination.Web.Authentication.Twitter;

namespace SubtleOstrich.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private string _twitterConsumerKey;
        private string _twitterConsumerSecret;
        private string _facebookAppId;
        private string _facebookAppSecret;
        private string _googleConsumerKey;
        private string _googleConsumerSecret;

        protected void Application_Start()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _twitterConsumerKey = appSettings["TwitterKey"];
            _twitterConsumerSecret = appSettings["TwitterSecret"];
            _facebookAppId = appSettings["FacebookKey"];
            _facebookAppSecret = appSettings["FacebookSecret"];
            _googleConsumerKey = appSettings["GoogleKey"];
            _googleConsumerSecret = appSettings["GoogleSecret"];

            AreaRegistration.RegisterAllAreas();

            WorldDominationRouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(typeof(WorldDominationAuthenticationController).Assembly);

            ConfigureWorldDomination(builder);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void ConfigureWorldDomination(ContainerBuilder builder)
        {
            var twitterProvider = new TwitterProvider(_twitterConsumerKey, _twitterConsumerSecret);
            var facebookProvider = new FacebookProvider(_facebookAppId, _facebookAppSecret);
            var googleProvider = new GoogleProvider(_googleConsumerKey, _googleConsumerSecret);

            var authenticationService = new AuthenticationService();

            authenticationService.AddProvider(twitterProvider);
            authenticationService.AddProvider(facebookProvider);
            authenticationService.AddProvider(googleProvider);

            builder.RegisterInstance(authenticationService).As<IAuthenticationService>();
            builder.RegisterType<AuthenticationCallbackProvider>().As<IAuthenticationCallbackProvider>();
        }  
    }

    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly IUserRepository _repository;

        public AuthenticationCallbackProvider()
            : this(new UserRepository())
        {
        }

        public AuthenticationCallbackProvider(IUserRepository repo)
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

            return new ViewResult();
        }
    }

    //public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    //{
    //    private IUserRepository _repository;

    //    public AuthenticationCallbackProvider()
    //        :this(new UserRepository())
    //    {
    //    }

    //    public AuthenticationCallbackProvider(IUserRepository repo)
    //    {
    //        _repository = repo;
    //    }

    //    public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
    //    {
    //        var u = _repository.GetBySourceAndId(model.AuthenticatedClient.ProviderName, model.AuthenticatedClient.UserInformation.Id);

    //        if(u == null)
    //        {
    //            u = new User(model.AuthenticatedClient.UserInformation.Id, model.AuthenticatedClient.UserInformation.Name, model.AuthenticatedClient.ProviderName);
    //            u.Save();
    //        }
            
    //        return nancyModule.Negotiate.WithView("Home").WithModel(u);
    //    }
    //}
}