using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;

var builder = WebApplication.CreateBuilder(args);
var samlConfiguration = builder.Configuration.GetSection("AdamAppSerice");

builder.Services.AddAuthentication(sharedOptions =>
{
    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    sharedOptions.DefaultChallengeScheme = "Saml2";
})
.AddSaml2(options =>
{
    options.SPOptions.EntityId = new EntityId(samlConfiguration["EntityId"]);
    options.SPOptions.PublicOrigin = new Uri(samlConfiguration["PublicOrigin"]);
    options.SPOptions.ReturnUrl = new Uri(samlConfiguration["ReturnUrl"]);
    var identityProvider = new IdentityProvider(new EntityId(samlConfiguration["IpEntityId"]), options.SPOptions)
    {
        // url or relative path "~/metadata.xml"
        MetadataLocation = samlConfiguration["IpMetadataUrl"],
        // load metadata must follow the metadata location
        LoadMetadata = true,
    };
    options.IdentityProviders.Add(identityProvider);
})
.AddCookie(options =>
{
    // Cookie configuration to share cookie with a .NET Framework WebForms application
    options.Cookie.Name = ".AspNet.SharedCookie";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Path = "/";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
    options.CookieManager = new ChunkingCookieManager();
    options.TicketDataFormat = new SecureDataFormat<AuthenticationTicket>(
        new TicketSerializer(),
        DataProtectionProvider.Create(
                new DirectoryInfo("fileshare path"),
                (builder) => { builder.SetApplicationName("iis-app-name"); })
            .CreateProtector(
                "Microsoft.AspNetCore.Authentication.Cookies." +
                "CookieAuthenticationMiddleware",
                "Cookies.Application",
                "v2"));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.Run(async context =>
{
    if (!context.User.Identity!.IsAuthenticated)
    {
        await context.ChallengeAsync();
    }
    else
    {
        var returnUrl = context.Request.Query["returnurl"];
        if (!string.IsNullOrEmpty(returnUrl))
        {
            context.Response.Redirect(returnUrl);
        }
    }
});
app.Run();