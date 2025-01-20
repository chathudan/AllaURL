using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new HttpClient();

        // Discover the endpoints from your IdentityServer
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000"); // Change to your IdentityServer URL
        if (disco.IsError)
        {
            Console.WriteLine($"Error: {disco.Error}");
            return;
        }

        // Request a token using the client credentials flow (or code flow)
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "allaurl-api", // Your client ID
            ClientSecret = "your-secret", // Your client secret
            Scope = "api" // The scope you want to request
        };

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest);

        if (tokenResponse.IsError)
        {
            Console.WriteLine($"Error: {tokenResponse.Error}");
            return;
        }

        // Print the obtained access token
        Console.WriteLine($"Access Token: {tokenResponse.AccessToken}");
    }
}
