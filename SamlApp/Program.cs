using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddCookiePolicy(options =>
{
    options.Secure = CookieSecurePolicy.Always;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

var samlConfiguration = builder.Configuration.GetSection("GCDS-new");

builder.Services.AddAuthentication(sharedOptions =>
{
    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    sharedOptions.DefaultChallengeScheme = "Saml2";
})
.AddSaml2(options =>
{
    options.SPOptions.EntityId = new EntityId(samlConfiguration["EntityId"]);
    options.SPOptions.ReturnUrl = new Uri(samlConfiguration["ReturnUrl"]);
    // Mechanism to modify the NameClaimType
    options.Notifications.AcsCommandResultCreated = (commandResult, saml2Response) =>
    {
        if (commandResult.Principal is not null && commandResult.Principal.Identity is ClaimsIdentity identity)
        {
            var edipi = identity.Claims.Single(x => x.Type == "edipi");
            var nameClaim = new Claim(ClaimTypes.Name, edipi.Value);
            identity.AddClaim(nameClaim);
        }
    };
    if (!builder.Environment.IsDevelopment())
    {
        // This does not need to be configured for local development
        // This MAY need to be configured to run on Azure App Service in a RP scenario
        options.SPOptions.PublicOrigin = new Uri(samlConfiguration["PublicOrigin"]);
    }
    var identityProvider = new IdentityProvider(new EntityId(samlConfiguration["IpEntityId"]), options.SPOptions)
    {
        MetadataLocation = samlConfiguration["IpMetadataUrl"], //"~/metadata.xml",
        LoadMetadata = true,
    };
    options.IdentityProviders.Add(identityProvider);
})
.AddCookie();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();