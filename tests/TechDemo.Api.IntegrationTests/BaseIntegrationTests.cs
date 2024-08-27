namespace TechDemo.Api.IntegrationTests;

public abstract class BaseIntegrationTests : IClassFixture<WebAppFactory>
{

    public BaseIntegrationTests(WebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }

    protected HttpClient HttpClient { get; set; }
}