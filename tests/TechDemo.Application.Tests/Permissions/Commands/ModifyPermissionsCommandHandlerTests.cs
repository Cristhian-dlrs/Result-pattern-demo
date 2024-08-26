using Moq;
using TechDemo.Application.Permissions.Commands;
using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Tests.Permissions.Commands;

public class ModifyPermissionsCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPermissionsRepository> _permissionsRepositoryMock;

    public ModifyPermissionsCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Loose);
        _permissionsRepositoryMock = new Mock<IPermissionsRepository>(MockBehavior.Loose);
    }

    [Fact]
    public void HandleAsync_should_return_isSuccess_true_when_permission_is_modified()
    {
        // Arrange
        var command = new ModifyPermissionsCommand(Guid.NewGuid(), "John", "Doe", "Admin");
        var permission = Permission.Create("Test", "Tester", "Admin").Value;

        _unitOfWorkMock.Setup(
                unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        _unitOfWorkMock.Setup(
                unitOfWork => unitOfWork.PermissionsRepository)
            .Returns(_permissionsRepositoryMock.Object);

        _permissionsRepositoryMock.Setup(
                permissionsRepository => permissionsRepository.GetByIdAsync(
                    It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Permission>.Success(permission));

        _permissionsRepositoryMock.Setup(
                permissionsRepository => permissionsRepository.Update(It.IsAny<Permission>()))
            .Returns(Result.Success());

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
        var command = new ModifyPermissionsCommand(Guid.NewGuid(), "John", "Doe", "Admin");
        var expectedError = Error.NullValue;

        _unitOfWorkMock.Setup(
                unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        _unitOfWorkMock.Setup(
                unitOfWork => unitOfWork.PermissionsRepository)
            .Returns(_permissionsRepositoryMock.Object);

        _permissionsRepositoryMock.Setup(
                permissionsRepository => permissionsRepository.GetByIdAsync(
                    It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Permission>.Failure(expectedError));

        _permissionsRepositoryMock.Setup(
                permissionsRepository => permissionsRepository.Update(It.IsAny<Permission>()))
            .Returns(Result.Success());

        var sender = TestHelpers.BuildSender(_unitOfWorkMock);

        // Act
        var actual = sender.Send(command);

        // Assert
        Assert.True(actual.Result.IsFailure && actual.Result.Error == expectedError);
    }
}