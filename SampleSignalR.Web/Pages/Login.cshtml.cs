using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleSignalR.Web.Pages
{
    public class LoginModel : PageModel
    {
        private static Dictionary<string, string> UserList = new();

        #region LOGIN

        [BindProperty]
        public string LoginUserName { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string LoginPassword { get; set; }

        #endregion

        #region REGISTER

        [BindProperty]
        public string RegisterUserName { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string RegisterPassword { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string RegisterComparePassword { get; set; }

        #endregion


        public void OnGet()
        {
            if (!UserList.Any())
                UserList.Add("umit@mail.com", "123456");
        }

        public async Task<IActionResult> OnPostLogin()
        {
            var anyUser = UserList.Any(u => u.Key.Equals(LoginUserName)
                                         && u.Value.Equals(LoginPassword)
                                      );
            if (!anyUser)
            {
                ModelState.AddModelError(string.Empty, "Username or password is incorrect !! ");

                return Page();
            }

            return await Task.FromResult(
                RedirectToPage("/Index")
            );
        }

        public async Task<IActionResult> OnPostRegister()
        {
            var isCompare = RegisterPassword == RegisterComparePassword;

            if (!isCompare)
            {
                ModelState.AddModelError(string.Empty, "Passwords entered do not match !! ");

                return Page();
            }

            return await Task.FromResult(
                RedirectToPage("/Index")
            );
        }
    }
}
