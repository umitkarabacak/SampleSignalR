using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleSignalR.Web.Pages
{
    public class LogoutModel : PageModel
    {

        public async Task<IActionResult> OnGet()
        {

            return await Task.FromResult(
                RedirectToPage("/login")
            );
        }
    }
}
