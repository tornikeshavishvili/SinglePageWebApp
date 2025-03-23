using System;
using System.Web;

namespace SinglePageWebApp
{
    public class SessionHttpModule : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.BeginRequest += OnBeginRequest;
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }

        public void Dispose() { }
    }
}
