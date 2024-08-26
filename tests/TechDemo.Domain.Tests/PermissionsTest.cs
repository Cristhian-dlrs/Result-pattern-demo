using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Permissions.Models.Events;

namespace TechDemo.Domain.Tests;

public class PermissionsTests
{
    public class Given_valid_input
    {
        [Fact]
        public void Crate_permissions_should_return_a_new_permission_successfully()
        {
            // Arrange
            var employeeForename = "John";
            var employeeSurname = "Doe";
            var permissionType = "Owner";

            // Act
            var actual = Permission.Create(employeeForename, employeeSurname, permissionType);

            // Assert
            Assert.True(
                actual.IsSuccess &&
                actual.Value.DomainEvents.Count == 1 &&
                actual.Value.DomainEvents.First().Operation == nameof(Operations.Request));
        }

        [Theory]
        [InlineData("Test", "Test", "Admin")]
        [InlineData("Test", null, null)]
        [InlineData(null, "Test", null)]
        public void Modify_permissions_should_update_permission_state_successfully(
            string? employeeForename, string? employeeSurname, string? permissionType)
        {
            // Arrange
            var permission = Permission.Create("John", "Doe", "Owner").Value;
            permission.FlushDomainEvents();

            // Act
            var actual = permission.ModifyPermission(employeeForename, employeeSurname, permissionType);

            // Assert
            Assert.True(actual.IsSuccess &&
                permission.DomainEvents.Count == 1 &&
                permission.DomainEvents.First().Operation == nameof(Operations.Modify));
        }
    }

    public class Given_invalid_input
    {
        [Theory]
        [InlineData("", "Test", "Admin", "Permissions.InvalidEmployeeForename")]
        [InlineData("Test", "", "Owner", "Permissions.InvalidEmployeeSurname")]
        [InlineData("Test", "Test", "", "Permissions.InvalidPermissionType")]
        public void Crate_permissions_should_return_error(
            string employeeForename, string employeeSurname, string permissionType, string expectedErrorCode)
        {
            // Arrange && Act
            var actual = Permission.Create(employeeForename, employeeSurname, permissionType);

            // Assert
            Assert.True(
                actual.IsFailure &&
                actual.Error.Code == expectedErrorCode);
        }

        [Theory]
        [InlineData("", "Test", "Admin", "Permissions.InvalidEmployeeForename")]
        [InlineData("Test", "", "Owner", "Permissions.InvalidEmployeeSurname")]
        [InlineData("Test", "Test", "", "Permissions.InvalidPermissionType")]
        public void Modify_should_return_error(
            string employeeForename, string employeeSurname, string permissionType, string expectedErrorCode)
        {
            // Arrange 
            var permission = Permission.Create("John", "Doe", "Owner").Value;
            permission.FlushDomainEvents();

            // Act
            var actual = Permission.Create(employeeForename, employeeSurname, permissionType);

            // Assert
            Assert.True(
                actual.IsFailure &&
                permission.DomainEvents.Count == 0 &&
                actual.Error.Code == expectedErrorCode);
        }
    }
}