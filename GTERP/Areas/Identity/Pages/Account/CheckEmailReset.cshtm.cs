using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GTRChitra.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class CheckEmailResetModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
