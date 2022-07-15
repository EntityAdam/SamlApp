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
            if (User.Claims.Any())
            {
                Claims = User.Claims;
            }
        }
    }
}

