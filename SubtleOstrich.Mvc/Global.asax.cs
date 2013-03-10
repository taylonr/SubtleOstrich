using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using SubtleOstrich.Mvc.Controllers;
using WorldDomination.Web.Authentication;
using WorldDomination.Web.Authentication.Facebook;
using WorldDomination.Web.Authentication.Google;
using WorldDomination.Web.Authentication.Mvc;
using WorldDomination.Web.Authentication.Twitter;

namespace SubtleOstrich.Mvc.Controllers
{
}

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
            builder.RegisterType<AuthenticationController>().As<IAuthenticationCallbackProvider>();
        }  
    }
}