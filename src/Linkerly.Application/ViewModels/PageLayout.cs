using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Linkerly.Application.ViewModels;

public class PageLayout : LayoutComponentBase
{
    [Inject]
    public required AuthenticationStateProvider AuthenticationStateProvider { get; set; }


    protected override async Task OnInitializedAsync()
    {
        AuthenticationState authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        ClaimsPrincipal claimsPrincipal = authenticationState.User;

        ICollection<Claim> claims = claimsPrincipal.Claims.ToList();
    }

    public bool IsSidebarVisible { get; set; }

    public void ToggleSidebar()
    {
        IsSidebarVisible = !IsSidebarVisible;
    }
}