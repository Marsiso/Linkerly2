using System.Security.Claims;
using Linkerly.Core.Application.Users.Commands;
using Linkerly.Core.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Linkerly.Application.Authentication;

public class BlazorAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BlazorAuthenticationStateProvider> _logger;
    private readonly ISender _messageHandlerBroker;

    public BlazorAuthenticationStateProvider(ISender messageHandlerBroker, IHttpContextAccessor httpContextAccessor, ILogger<BlazorAuthenticationStateProvider> logger)
    {
        _messageHandlerBroker = messageHandlerBroker;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null) return GetAnonymousAuthenticationState();

        var claimsPrincipal = httpContext.User;

        try
        {
            var getUserQuery = new GetUserByIdentifierQuery(claimsPrincipal.FindFirstValue("id"));

            var originalUser = await _messageHandlerBroker.Send(getUserQuery);

            if (originalUser is null)
            {
                var createUserCommand = new CreateUserCommand(
                    claimsPrincipal.FindFirstValue("id"),
                    claimsPrincipal.FindFirstValue("email"),
                    "true".Equals(claimsPrincipal.FindFirstValue("verified_email"), StringComparison.OrdinalIgnoreCase),
                    claimsPrincipal.FindFirstValue("name"), claimsPrincipal.FindFirstValue("given_name"),
                    claimsPrincipal.FindFirstValue("family_name"),
                    claimsPrincipal.FindFirstValue("picture"),
                    claimsPrincipal.FindFirstValue("locale"));

                _ = await _messageHandlerBroker.Send(createUserCommand);
            }
            else
            {
                var updateUserCommand = new UpdateUserCommand(
                    originalUser.UserID,
                    claimsPrincipal.FindFirstValue("id"),
                    claimsPrincipal.FindFirstValue("email"),
                    "true".Equals(claimsPrincipal.FindFirstValue("verified_email"), StringComparison.OrdinalIgnoreCase),
                    claimsPrincipal.FindFirstValue("name"), claimsPrincipal.FindFirstValue("given_name"),
                    claimsPrincipal.FindFirstValue("family_name"),
                    claimsPrincipal.FindFirstValue("picture"),
                    claimsPrincipal.FindFirstValue("locale"));

                _ = await _messageHandlerBroker.Send(updateUserCommand);
            }

            return new AuthenticationState(claimsPrincipal);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
        }

        return GetAnonymousAuthenticationState();
    }

    private static AuthenticationState GetAnonymousAuthenticationState()
    {
        var claimsIdentity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authenticationState = new AuthenticationState(claimsPrincipal);

        return authenticationState;
    }
}