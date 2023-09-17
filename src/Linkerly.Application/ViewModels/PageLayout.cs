using System.Security.Claims;
using Linkerly.Core.Application.Users.Queries;
using Linkerly.Domain.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Linkerly.Application.ViewModels;

public class PageLayout : LayoutComponentBase
{
    [Inject] public required AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    [Inject] public required ISender _messageHandlerBroker { get; set; }

    public UserEntity? User { get; set; }

    public bool IsSidebarVisible { get; set; }


    protected override async Task OnInitializedAsync()
    {
        AuthenticationState authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        ClaimsPrincipal claimsPrincipal = authenticationState.User;

        GetUserByIdentifierQuery getUserQuery = new GetUserByIdentifierQuery(claimsPrincipal.FindFirstValue("id"));

        User = await _messageHandlerBroker.Send(getUserQuery);
    }

    public void ToggleSidebar()
    {
        IsSidebarVisible = !IsSidebarVisible;
    }
}