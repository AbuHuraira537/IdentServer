using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentServer
{
    public static class Configurations
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                
                new IdentityResource
                {
                    Name="user.scope",
                    UserClaims =
                    {
                        "my.claim"
                    }
                }
            };
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("ApiOne","ApiOne",new string[]{ "Mycoooookie.big"}),
                new ApiResource("all"),
            };
        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope("ApiOne","ApiOne"),
                new ApiScope("all"),
                
            };
        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId="client_id",
                    ClientSecrets={new Secret("client_serect".ToSha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={"ApiOne","all"}
                },
                new Client
                {
                    ClientId="client_id_mvc",
                    ClientSecrets={new Secret("client_serect_mvc".ToSha256())},
                    AllowedGrantTypes=GrantTypes.Code,
                    RedirectUris = { "https://localhost:44362/signin-oidc" },
                    AllowedScopes =new List<string>
                    {
                        "ApiOne",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "offline_access",
                    },
                   
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    //this line will automatically include all the claims of user mention in identityresource
                    //these claims stored in id_token if claims increases idtoken size also increase
                    // AlwaysIncludeUserClaimsInIdToken =true,
                    RequireConsent = false,
                   
                    
                },
                 new Client {
                    ClientId = "client_id_js",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    //RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "https://localhost:44312/home/signin" },
                    PostLogoutRedirectUris = { "https://localhost:44312/Home/Index" },
                    AllowedCorsOrigins = { "https://localhost:44312" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                        "ApiTwo",
                        "rc.scope",
                    },

                    AccessTokenLifetime = 1,

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },
                new Client {
                    
                    ClientId = "react",
                    AllowedGrantTypes = GrantTypes.Code,

                     RequireClientSecret = false,

                     RedirectUris = { "https://localhost:44380" },
                    PostLogoutRedirectUris = { "https://localhost:44380" },
                    AllowedCorsOrigins = { "https://localhost:44380" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                 
                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },
            };
    }
}
