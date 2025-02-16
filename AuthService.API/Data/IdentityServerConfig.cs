using Duende.IdentityServer.Models;

namespace AuthService.API.Data
{
    public static class IdentityServerConfig
    {
            // File Service API'ye erişim sağlayacak istemci
            public static IEnumerable<Client> Clients =>
                new List<Client>
                {
                new Client
                {
                    ClientId = "file-service-client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // M2M iletişim için client credentials akışı
                    ClientSecrets = { new Secret("supersecret".Sha256()) }, // SHA-256 hash'li secret
                    AllowedScopes = { "file-service.read", "file-service.write" } // İzin verilen kapsamlar
                }
                };

            // File Service API için kapsam tanımlamaları
            public static IEnumerable<ApiScope> ApiScopes =>
                new List<ApiScope>
                {
                new ApiScope("file-service.read", "File Service API - Okuma Erişimi"),
                new ApiScope("file-service.write", "File Service API - Yazma Erişimi")
                };

            // File Service API kaynağı tanımı
            public static IEnumerable<ApiResource> ApiResources =>
                new List<ApiResource>
                {
                new ApiResource("file-service", "File Service API")
                {
                    Scopes = { "file-service.read", "file-service.write" }
                }
                };
        }
    }