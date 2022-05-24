using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Saml2App.WebForms.Backend
{
    public partial class Contact : Page
    {
        private readonly string auth_url = "https://nssiauthpoc.azurewebsites.net/auth?returnurl=";

        protected void Page_Load(object sender, EventArgs e)
        {
            var owincontext = Request.GetOwinContext();
            var claims = owincontext.Authentication.User.Claims;


            var user = HttpContext.Current.User;
            if (user.Identity.IsAuthenticated)
            {
                HttpContext.Current.Response.Write($"HttpContext Write Current User: '{user.Identity.Name}'\n");
                HttpContext.Current.Response.Write($"IsAuthenticated: {HttpContext.Current.User.Identity.IsAuthenticated}\n");
                HttpContext.Current.Response.Write($"AuthenticationType: {HttpContext.Current.User.Identity.AuthenticationType}\n");

                foreach(var c in claims)
                {
                    HttpContext.Current.Response.Write($"Claimn: {c.Issuer} - {c.Type} - {c.Value}\n");
                }

                HttpContext.Current.Response.End();
            }
            else
            {
                HttpContext.Current.Response.StatusCode = 401;
                HttpContext.Current.Response.Write("Nooope! Forbidden");
                HttpContext.Current.Response.End();
            }
        }
    }
}