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

        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Page("./Login", "Callback", new { returnUrl })
        };

        return new ChallengeResult(provider, authenticationProperties);
    }

    [HttpGet]
    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = default, string? remoteError = default)
    {
        var claimsIdentity = User.Identities.FirstOrDefault();

        var authenticated = claimsIdentity?.IsAuthenticated ?? false;

        if (!authenticated) return LocalRedirect(Routes.Index);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            RedirectUri = Request.Host.Value
        };

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity!);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

        return LocalRedirect(Routes.Index);
    }
}
