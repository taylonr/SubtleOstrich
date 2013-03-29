using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using Autofac;
using Autofac.Integration.Mvc;
using SubtleOstrich.Web.Controllers;
using WorldDomination.Web.Authentication;
using WorldDomination.Web.Authentication.Facebook;
using WorldDomination.Web.Authentication.Google;
using WorldDomination.Web.Authentication.Mvc;
using WorldDomination.Web.Authentication.Twitter;

namespace SubtleOstrich.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();

        //    WebApiConfig.Register(GlobalConfiguration.Configuration);
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);
        //    AuthConfig.RegisterAuth();
        //}

        private string _facebookAppId;
        private string _facebookAppSecret;
      
        protected void Application_Start()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _facebookAppId = appSettings["FacebookKey"];
            _facebookAppSecret = appSettings["FacebookSecret"];
    
            AreaRegistration.RegisterAllAreas();

            WorldDominationRouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(typeof(WorldDominationAuthenticationController).Assembly);

            ConfigureWorldDomination(builder);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        protected void ConfigureWorldDomination(ContainerBuilder builder)
        {
            var facebookProvider = new FacebookProvider(_facebookAppId, _facebookAppSecret);

            var authenticationService = new AuthenticationService();

            authenticationService.AddProvider(facebookProvider);

            builder.RegisterInstance(authenticationService).As<IAuthenticationService>();
            builder.RegisterType<AuthenticationController>().As<IAuthenticationCallbackProvider>();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializer = new JavaScriptSerializer();

                var serializeModel = serializer.Deserialize<CouchPrincipalSerializeModel>(authTicket.UserData);

                var newUser = new CouchPrincipal(authTicket.Name) {Uid = serializeModel.Uid, Source = serializeModel.Source, Picture = serializeModel.Picture};

                HttpContext.Current.User = newUser;
            }
        }
    }
}