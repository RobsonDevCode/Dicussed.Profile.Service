using System.Net.Http.Headers;
using System.Text.Json;
using Discussed.Profile.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Discussed.Profile.Domain.Clients;

public sealed class UserClient : IUserClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserClient> _logger;

    public UserClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<UserClient> logger)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<UserModel> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        var jwt = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (string.IsNullOrWhiteSpace(jwt))
        {
            throw new InvalidOperationException("Authorization header is missing.");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var response = await _httpClient.GetAsync($"v1/user/{userId}", cancellationToken);

        response.EnsureSuccessStatusCode();

        var user = await JsonSerializer.DeserializeAsync<UserModel>(
            await response.Content.ReadAsStreamAsync(cancellationToken), cancellationToken: cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return user;
    }
}