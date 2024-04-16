using System.Security.Claims;
using BusinessLayer.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using PresentationLayer.Constants;

namespace PresentationLayer.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCookiesAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;

                // Where to redirect browser if there is no active session
                options.LoginPath = "/api/Authentication/error";

                // Where to redirect browser if there ForbidResult acquired.
                options.AccessDeniedPath = "/api/Authentication/error";
            });

        return serviceCollection;
    }

    public static IServiceCollection AddRoles(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.AddPolicy(PolicyName.LeaderPolicy, policyBuilder =>
            {
                AccountRole[] allowedRoles = { AccountRole.Leader};
                policyBuilder
                    .RequireClaim(ClaimTypes.Role, allowedRoles.Select(x => x.ToString("G")))
                    .Build();
            });
            options.AddPolicy(PolicyName.OdrinaryEmployeePolicy, policyBuilder =>
            {
                AccountRole[] allowedRoles = { AccountRole.OrdinaryEmployee};
                policyBuilder
                    .RequireClaim(ClaimTypes.Role, allowedRoles.Select(x => x.ToString("G")))
                    .Build();
            });
        });
    }
}