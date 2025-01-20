using AllaURL.Identity.Models;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AllaURL.Identity.Pages.Create
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public Index(
            IIdentityServerInteractionService interaction,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _interaction = interaction;
        }

        public IActionResult OnGet(string? returnUrl)
        {
            Input = new InputModel { ReturnUrl = returnUrl ?? Url.Content("~/") };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

            // the user clicked the "cancel" button
            if (Input.Button != "create")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage(Input.ReturnUrl);
                    }

                    return Redirect(Input.ReturnUrl ?? "~/");
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (await _userManager.FindByNameAsync(Input.Username) != null)
            {
                ModelState.AddModelError("Input.Username", "Invalid username");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email
                };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // issue authentication cookie with subject ID and username
                    var isuser = new IdentityServerUser(user.Id)
                    {
                        DisplayName = user.UserName
                    };

                    await HttpContext.SignInAsync(isuser);

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage(Input.ReturnUrl);
                        }

                        // we can trust Input.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(Input.ReturnUrl ?? "~/");
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(Input.ReturnUrl))
                    {
                        return Redirect(Input.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(Input.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new ArgumentException("invalid return URL");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        public class InputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string ReturnUrl { get; set; }
            public string Button { get; set; }
        }
    }
}
