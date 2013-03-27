using System;
using System.Security.Principal;

namespace SubtleOstrich.Web.Controllers
{
    public class CouchPrincipal : IPrincipal
    {
        public CouchPrincipal(string name)
        {
            Identity = new GenericIdentity(name);
        }
      
        public string Uid { get; set; }
        public string Source { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity { get; private set; }
    }

    public class CouchPrincipalSerializeModel
    {
        public string Uid { get; set; }
        public string Source { get; set; }
        public string Name { get; set; }
    }
}