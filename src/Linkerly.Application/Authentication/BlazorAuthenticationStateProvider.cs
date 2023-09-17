using System.Security.Claims;
using AutoMapper;
using Linkerly.Core.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Linkerly.Application.Authentication;

public class BlazorAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BlazorAuthenticationStateProvider> _logger;
    private readonly IMapper _mapper;
    private readonly ISender _messageHandlerBroker;

    public BlazorAuthenticationStateProvider(ISender messageHandlerBroker, IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<BlazorAuthenticationStateProvider> logger)
    {
        _messageHandlerBroker = messageHandlerBroker;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            return GetAnonymousAuthenticationState();
        }

        ClaimsPrincipal claimsPrincipal = httpContext.User;

        CreateUserCommand createUserCommand = new CreateUserCommand(
            claimsPrincipal.FindFirstValue("id"),
            claimsPrincipal.FindFirstValue("email"),
            "true".Equals(claimsPrincipal.FindFirstValue("verified_email"), StringComparison.OrdinalIgnoreCase),
            claimsPrincipal.FindFirstValue("name"), claimsPrincipal.FindFirstValue("given_name"),
            claimsPrincipal.FindFirstValue("family_name"),
            claimsPrincipal.FindFirstValue("picture"));

        try
        {
            //_ = await _messageHandlerBroker.Send(createUserCommand);

            //GetUserByEmailQuery getUserQuery = new();

            //Domain.Application.Models.UserEntity? originalUser = await _messageHandlerBroker.Send(getUserQuery);

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
        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        AuthenticationState authenticationState = new AuthenticationState(claimsPrincipal);

        return authenticationState;
    }
}