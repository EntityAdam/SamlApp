using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;

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
    options.SPOptions.PublicOrigin = new Uri(samlConfiguration["PublicOrigin"]);
    options.SPOptions.ReturnUrl = new Uri(samlConfiguration["ReturnUrl"]);
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