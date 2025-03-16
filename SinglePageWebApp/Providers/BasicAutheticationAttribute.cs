 

using Microsoft.AspNet.Identity.Owin;
using SinglePageWebApp;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

public class BasicAuthenticationAttribute : AuthorizeAttribute
{
    protected override bool IsAuthorized(HttpActionContext actionContext)
    {


        if ((HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated) ||
            (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity.IsAuthenticated))
        {
            return true;
        }

        // Look for the Authorization header
        var authHeader = actionContext.Request.Headers.Authorization;
        if (authHeader != null && authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
        {
            // Decode the credentials (should be in the format "username:password")
            var credentials = ParseCredentials(authHeader.Parameter);
            if (credentials != null)
            {
                // Validate the credentials (replace this with your actual validation)
                if (ValidateUser(credentials.Item1, credentials.Item2))
                {
                    // If valid, create an identity and attach it to the current principal
                    var identity = new GenericIdentity(credentials.Item1);
                    var principal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = principal;
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }
                    return true;
                }
            }
        }
        return false;
    }

    private Tuple<string, string> ParseCredentials(string parameter)
    {
        try
        {
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(parameter));
            var parts = decodedCredentials.Split(new[] { ':' }, 2);
            if (parts.Length == 2)
            {
                return Tuple.Create(parts[0], parts[1]);
            }
        }
        catch
        {
            // Log error if needed
        }
        return null;
    }

    private bool ValidateUser(string username, string password)
    {
        // Get the UserManager from the OWIN context.
        var owinContext = HttpContext.Current.GetOwinContext();
        var userManager = owinContext.GetUserManager<ApplicationUserManager>();

        // Validate the user using ASP.NET Identity same as the login page
        var user = userManager.FindByEmailAsync(username).Result;
        if (user != null && userManager.CheckPasswordAsync(user, password).Result)
        {
            // Optionally add roles if needed:
            var identity = new GenericIdentity(username);
            // You can add roles from userManager.GetRoles(user.Id) if necessary.
            var principal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            return true;
        }
        return false;
    }

    protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
    { 

        // Create a 401 Unauthorized response
        var response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

        // Add the WWW-Authenticate header to trigger the browser's Basic Auth dialog
        response.Headers.Add("WWW-Authenticate", "Basic realm=\"My API\"");

        // Set the response
        actionContext.Response = response;
    }

}