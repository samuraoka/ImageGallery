using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Marvin.IDP
{
    public static class Config
    {
        // Adding users
        // http://docs.identityserver.io/en/release/quickstarts/2_resource_owner_passwords.html#adding-users
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "Frank",
                    Password = "password",

                    // Further experiments
                    // http://docs.identityserver.io/en/release/quickstarts/3_interactive_login.html?highlight=claim#further-experiments
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Frank"),
                        new Claim("family_name", "Underwood"),
                    },
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Claire",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Claire"),
                        new Claim("family_name", "Underwood"),
                    },
                },
            };
        }

        // Adding support for OpenID Connect Identity Scopes
        // http://docs.identityserver.io/en/release/quickstarts/3_interactive_login.html#adding-support-for-openid-connect-identity-scopes
        //
        // 5.4.  Requesting Claims using Scope Values
        // https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        // Defining the client
        // http://docs.identityserver.io/en/release/quickstarts/1_client_credentials.html#defining-the-client
        //
        // Modifying the client configuration
        // http://docs.identityserver.io/en/release/quickstarts/5_hybrid_and_api_access.html#modifying-the-client-configuration
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>();
        }
    }
}
