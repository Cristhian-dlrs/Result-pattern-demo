using Moq;
using TechDemo.Application.Permissions.Commands;
using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Tests.Permissions.Commands;

public class RequestPermissionsCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPermissionsRepository> _permissionsRepositoryMock;

    public RequestPermissionsCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Loose);
        _permissionsRepositoryMock = new Mock<IPermissionsRepository>(MockBehavior.Loose);
    }

    [Fact]
    public void HandleAsync_should_return_isSuccess_true_when_permission_is_created()
    {
        // Arrange
        var command = new RequestPermissionsCommand("John", "Doe", "Admin");

        _unitOfWorkMock.Setup(
                unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        _unitOfWorkMock.Setup(
                unitOfWork => unitOfWork.PermissionsRepository)
            .Returns(_permissionsRepositoryMock.Object);

        _permissionsRepositoryMock.Setup(
                permissionsRepository => permissionsRepository.CreateAsync(
                    It.IsAny<Permission>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var sender = TestHelpers.BuildSender(_unitOfWorkMock);

        // Act
        var actual = sender.Send(command);

        // Assert
        Assert.True(actual.Result.IsSuccess);
    }

    [Fact]
    public void HandleAsync_should_return_isFailure_true_when_error()
    {
        // Arrange
        var command = new RequestPermissionsCommand("John", "Doe", "Admin");
        var expectedError = Error.NullValue;

        _unitOfWorkMock.Setup(
                unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        _unitOfWorkMock.Setup(
                unitOfWork => unitOfWork.PermissionsRepository)
            .Returns(_permissionsRepositoryMock.Object);

        _permissionsRepositoryMock.Setup(
                permissionsRepository => permissionsRepository.CreateAsync(
                    It.IsAny<Permission>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(expectedError));

        var sender = TestHelpers.BuildSender(_unitOfWorkMock);

        // Act
        var actual = sender.Send(command);

        // Assert
        Assert.True(actual.Result.IsFailure && actual.Result.Error == expectedError);
    }
}