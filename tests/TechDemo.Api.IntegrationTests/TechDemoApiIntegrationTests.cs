using System.Net;
using System.Net.Http.Json;
using TechDemo.Api.Permissions.Requests;

namespace TechDemo.Api.IntegrationTests;

public class TechDemoApiIntegrationTests : BaseIntegrationTests
{
    public TechDemoApiIntegrationTests(WebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreatePermissions_endpoint_should_return_201_when_permission_is_created()
    {
        // Arrange
        var request = new CreatePermissionRequest("John", "Doe", "Admin");

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/v1/permissions", request);

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.Created);
    }
}