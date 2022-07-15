using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Interop;
using Owin;
using Sustainsys.Saml2.Owin;
using System;
using System.IO;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Saml2App.WebForms.Backend.Startup))]

namespace Saml2App.WebForms.Backend
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Cookie Configuration to share cookies with AspNetCore
            //app.UseCookieAuthentication(new CookieAuthenticationOptions()
            //{
            //    AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
            //    CookieName = ".AspNet.SharedCookie",
            //    CookieSameSite = SameSiteMode.Lax,
            //    SlidingExpiration = true,
            //    ExpireTimeSpan = TimeSpan.FromMinutes(120),
            //    LoginPath = PathString.FromUriComponent("/auth"),
            //    //LogoutPath = PathString.FromUriComponent("/auth/logout"),
            //    TicketDataFormat = new AspNetTicketDataFormat(
            //                        new DataProtectorShim(
            //                            DataProtectionProvider.Create(
            //                                new DirectoryInfo("fileshare path"),
            //                                (builder) =>
            //                                {
            //                                    builder.SetApplicationName("iis-app-name");
            //                                })
            //                            .CreateProtector(
            //                                "Microsoft.AspNetCore.Authentication.Cookies." +
            //                                "CookieAuthenticationMiddleware",
            //                                "Cookies.Application",
            //                                "v2"))),
            //    CookieManager = new ChunkingCookieManager()
            //});
        }
    }
}
