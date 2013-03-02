using System.Configuration;
using Nancy;
using Nancy.TinyIoc;
using WorldDomination.Web.Authentication;
using WorldDomination.Web.Authentication.Facebook;
using WorldDomination.Web.Authentication.Google;
using WorldDomination.Web.Authentication.Twitter;

namespace SubtleOstrich.Api
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly string _twitterConsumerKey;
        private readonly string _twitterConsumerSecret;
        private readonly string _facebookAppId;
        private readonly string _facebookAppSecret;
        private readonly string _googleConsumerKey;
        private readonly string _googleConsumerSecret;

        public Bootstrapper()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _twitterConsumerKey = appSettings["TwitterKey"];
            _twitterConsumerSecret = appSettings["TwitterSecret"];
            _facebookAppId = appSettings["FacebookKey"];
            _facebookAppSecret = appSettings["FacebookSecret"];
            _googleConsumerKey = appSettings["GoogleKey"];
            _googleConsumerSecret = appSettings["GoogleSecret"];
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var twitterProvider = new TwitterProvider(_twitterConsumerKey, _twitterConsumerSecret);
            var facebookProvider = new FacebookProvider(_facebookAppId, _facebookAppSecret);
            var googleProvider = new GoogleProvider(_googleConsumerKey, _googleConsumerSecret);

            var authenticationService = new AuthenticationService();

            authenticationService.AddProvider(twitterProvider);
            authenticationService.AddProvider(facebookProvider);
            authenticationService.AddProvider(googleProvider);

            container.Register<IAuthenticationService>(authenticationService);
        }
    }
}