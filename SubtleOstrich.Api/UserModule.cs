using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.ModelBinding;
using SubtleOstrich.Logic;

namespace SubtleOstrich.Api
{
    public class UserModule : NancyModule
    {
        public UserModule() : base("/User")
        {
            Post["/"] = Save;
        }

        private Response Save(dynamic o)
        {
            var u = this.Bind<User>();
            u.Save();
            return HttpStatusCode.OK;
        }
    }
}