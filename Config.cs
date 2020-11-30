// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;
using IdentityModel;

namespace Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Address(),
                new IdentityResources.Profile(),
                new IdentityResource("roles","角色",new List<string>{ JwtClaimTypes.Role}),
                new IdentityResource("locations","地点", new List<string>{ "location"})
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("api1", "My API #1", new List<string>{ "location"})
                {
                    ApiSecrets = { new Secret("api1 secret".Sha256())}
                },
                new ApiResource("api2", "Express API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "console client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "api1", "api2"}
                },
                //wpf client password 
                new Client
                {
                    ClientId = "wpf client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets ={
                        new Secret("wpf secrect".Sha256())
                    },
                    AllowedScopes = {
                        "api1",
                        "api2",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Profile}
                },
                //mvc client  
                new Client
                {
                    ClientId = "mvc client",
                    ClientName = "ASP.NET Core MVC Client",

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientSecrets ={ new Secret("mvc secret".Sha256()) },
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 60 ,//60秒刷新,默认一个小时

                    AllowedScopes = {
                        "api1",
                        "api2",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                //angular, implicit flow
                new Client
                {
                    ClientId = "angular-client",
                    ClientName =  "Angular SPA 客户端",
                    ClientUri = "http://localhost:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = true,
                    AccessTokenLifetime = 60 * 5,

                    RedirectUris =
                    {
                        "http://localhost:4200/signin-oidc",
                        "http://localhost:4200/redirect-silentrenew"
                    },

                    PostLogoutRedirectUris =
                    {
                        "http://localhost:4200"
                    },

                    AllowedCorsOrigins =
                    {
                        "http://localhost:4200"
                    },

                    AllowedScopes = {
                        "api1",
                        "api2",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                //mvc hybrid flow
                new Client
                {
                    ClientId = "hybrid client",
                    ClientName = "ASP.NET Core Hybrid 客户端",
                    ClientSecrets = { new Secret("hybrid secret".Sha256())},

                    AllowedGrantTypes = GrantTypes.Hybrid,
                    //AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    //默认Jwt
                    AccessTokenType = AccessTokenType.Reference,

                    RedirectUris =
                    {
                        "http://localhost:7000/signin-oidc"
                    },
                    FrontChannelLogoutUri = "http://localhost:7000/signout-oidc",
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:7000/signout-callback-oidc"
                    },
                    AllowOfflineAccess = true,
                    //AccessTokenLifetime = 60 ,//60秒刷新,默认一个小时

                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedScopes = {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "locations"
                    }
                },
                
                //python Flash client using authorization code flow
                new Client
                {
                    ClientId = "flask client",
                    ClientName = "Flask 客户端",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("flask secret".Sha256())},
                    Enabled = true,
                    RequireConsent = false,
                    AllowRememberConsent = false,
                    AccessTokenType = AccessTokenType.Jwt,
                    AlwaysIncludeUserClaimsInIdToken = false,
                    AllowOfflineAccess = true,

                    RedirectUris = { "http://localhost:7002/oidc_callback"},

                    AllowedScopes =
                    {
                        "api1",
                        "api2",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    }
                }

            };
        }
    }
}