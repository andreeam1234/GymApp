using System.Net;
using System.Net.Http.Json;
using GymApp.DTOs;
using Xunit;

namespace GymApp.Tests.Integration;

public class AuthApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithSeededAdminCredentials_ReturnsOkWithToken()
    {
        var dto = new LoginDto("admin@gymapp.com", "Admin@123");

        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.False(string.IsNullOrWhiteSpace(body!.Token));
        Assert.Contains("Admin", body.Roles);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        var dto = new LoginDto("admin@gymapp.com", "WrongPassword123");

        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithNonExistentEmail_ReturnsUnauthorized()
    {
        var dto = new LoginDto("nobody@gymapp.com", "Whatever123");

        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithNewEmail_CreatesAccountWithMemberRole()
    {
        var dto = new RegisterDto("new.user@gymapp.com", "New", "User", "Parola123");

        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.Contains("Member", body!.Roles);
        Assert.DoesNotContain("Admin", body.Roles);
    }

    [Fact]
    public async Task Register_WithAlreadyUsedEmail_ReturnsBadRequest()
    {
        var dto = new RegisterDto("admin@gymapp.com", "Some", "One", "Parola123");

        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private record LoginResponse(string Token, int ExpiresIn, string UserId, string Email, string FullName, List<string> Roles);
}