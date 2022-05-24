using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace SamlApp.SustainSys.Pages
{
    [Authorize]
    public class ClaimsModel : PageModel
    {
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
        public void OnGet()
        {
            var identity = User.Identity;
            if (identity is ClaimsIdentity claimsIdentityInstance)
            {
                Claims = claimsIdentityInstance.Claims.ToList();
            }
        }
    }
}

