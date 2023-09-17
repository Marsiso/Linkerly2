using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Linkerly.Application.ViewModels.Authentication;

[AllowAnonymous]
public class LoginViewModel : PageModel
{
    [HttpGet]
    public IActionResult OnGet(string? returnUrl = default)
    {
        const string provider = "Google";

        AuthenticationProperties authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Page("./Login", "Callback", new { returnUrl, }),
        };

        return new ChallengeResult(provider, authenticationProperties);
    }

    [HttpGet]
    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = default, string? remoteError = default)
    {
        ClaimsIdentity? user = User.Identities.FirstOrDefault();

        bool authenticated = user?.IsAuthenticated ?? false;

        if (!authenticated)
        {
            return LocalRedirect(Routes.Index);
        }

        AuthenticationProperties authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            RedirectUri = Request.Host.Value,
        };

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(user!);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

        return LocalRedirect(Routes.Index);
    }
}