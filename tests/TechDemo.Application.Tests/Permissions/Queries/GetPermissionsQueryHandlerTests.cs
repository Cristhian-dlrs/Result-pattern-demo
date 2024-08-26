using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechDemo.Application.Permissions.Queries;
using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Tests.Permissions.Queries;

public class GetPermissionsQueryHandlerTests
{
    private readonly Mock<IPermissionsViewRepository> _permissionsViewRepositoryMock;

    public GetPermissionsQueryHandlerTests()
    {
        _permissionsViewRepositoryMock = new Mock<IPermissionsViewRepository>(MockBehavior.Loose);
    }

    [Fact]
    public void HandleAsync_should_return_permissions_when_permission_when_query_success()
    {
        // Arrange
        var command = new GetPermissionsQuery("test");
        var permissions = new List<PermissionViewModel>
        {
            new( Guid.NewGuid(), "test1", "test1", "admin", DateTime.UtcNow),
            new( Guid.NewGuid(), "test2", "test2", "owner", DateTime.UtcNow),
        };

        _permissionsViewRepositoryMock.Setup(viewRepository => viewRepository.GetAsync(
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(permissions);

        var sender = BuildSender();

        // Act
        var actual = sender.Send(command);

        // Assert
        Assert.True(actual.Result.IsSuccess
            && actual.Result.Value.Permissions.Count() == permissions.Count());
    }

    [Fact]
    public void HandleAsync_should_return_isFailure_true_when_error()
    {
        // Arrange
        var command = new GetPermissionsQuery("test");
        var expectedError = Error.NullValue;

        _permissionsViewRepositoryMock.Setup(viewRepository => viewRepository.GetAsync(
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<PermissionViewModel>>.Failure(expectedError));

        var sender = BuildSender();

        // Act
        var actual = sender.Send(command);

        // Assert
        Assert.True(actual.Result.IsFailure && actual.Result.Error == expectedError);
    }

    private ISender BuildSender()
    {
        var services = new ServiceCollection();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationExtensions).Assembly))
            .AddSingleton(_permissionsViewRepositoryMock.Object);

        var provider = services.BuildServiceProvider();
        var sender = provider.GetRequiredService<ISender>();

        return sender;
    }
}