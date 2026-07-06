using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;

namespace Net10.docker.k8s.Services
{
    public interface ICognitoTokenService
    {
        Task<TokenResponseDto?> GetClientCredentialsTokenAsync(CancellationToken cancellationToken = default);
    }

    public class CognitoTokenService : ICognitoTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _tokenEndpoint;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _scope;

        public CognitoTokenService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            var section = _configuration.GetSection("Cognito");
            _tokenEndpoint = section.GetValue<string>("TokenEndpoint") ?? string.Empty;
            _clientId = section.GetValue<string>("ClientId") ?? string.Empty;
            _clientSecret = section.GetValue<string>("ClientSecret") ?? string.Empty;
            _scope = section.GetValue<string>("Scope") ?? string.Empty;
        }

        public async Task<TokenResponseDto?> GetClientCredentialsTokenAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_tokenEndpoint) || string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_clientSecret))
            {
                return null;
            }

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["scope"] = _scope
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint)
            {
                Content = new FormUrlEncodedContent(form)
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                // Could log or throw depending on desired behavior. Return null for now.
                return null;
            }

            var dto = JsonSerializer.Deserialize<TokenResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return dto;
        }
    }

    public class TokenResponseDto
    {
        public string? Access_Token { get; set; }
        public string? Token_Type { get; set; }
        public int Expires_In { get; set; }
        public string? Scope { get; set; }
    }
}
