using AllaURL.Domain.Common;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Security.Claims;

namespace AllaURL.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
               new IdentityResources.OpenId(),
               new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                  new ApiScope(IdentityScopes.AllaUrlApi, displayName: "AllaURL API")
            };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "AllaURL.API",
                ClientSecrets = { new Secret("your-secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials, // Use the Authorization Code Flow
                RedirectUris = { "http://localhost:5000/signin-oidc" }, // Redirect URL for your app
                PostLogoutRedirectUris = { "http://localhost:5000/signout-callback-oidc" },
                AllowedScopes = { "openid", "profile", "api" }, // Scopes the client can access
                RequirePkce = true, // Use PKCE for better security
                AllowAccessTokensViaBrowser = true
            }
        };

}