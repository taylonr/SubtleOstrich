﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using SubtleOstrich.Logic;
using WorldDomination.Web.Authentication.Mvc;

namespace SubtleOstrich.Web.Controllers
{
    public class AuthenticationController : Controller, IAuthenticationCallbackProvider
    {
        private readonly IUserRepository _repository;

        public AuthenticationController()
            : this(new UserRepository())
        {
        }

        public AuthenticationController(IUserRepository repo)
        {
            _repository = repo;
        }

        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model)
        {
            var u = _repository.GetBySourceAndId(model.AuthenticatedClient.ProviderName, model.AuthenticatedClient.UserInformation.Id);

            if (u == null)
                u = Logic.User.CreateUser(model.AuthenticatedClient.UserInformation.Id, model.AuthenticatedClient.UserInformation.Name, model.AuthenticatedClient.ProviderName);

            var serializeModel = new CouchPrincipalSerializeModel();
            var id = u.Id.Split(':');

            serializeModel.Uid = id[1];
            serializeModel.Source = id[0];
            serializeModel.Name  = u.Name;
            serializeModel.Picture = model.AuthenticatedClient.UserInformation.Picture;

            var serializer = new JavaScriptSerializer();

            var userData = serializer.Serialize(serializeModel);

            var authTicket = new FormsAuthenticationTicket(
                     1,
                     u.Name,
                     DateTime.Now,
                     DateTime.Now.AddYears(1),
                     false,
                     userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            context.Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Activity");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Activity");
        }
    }
}
