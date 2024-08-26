using TechDemo.Domain.Permissions.Models;

namespace TechDemo.Domain.Tests.Permissions;

public class PermissionsTypeTests
{
    public class Given_valid_permissionType_data
    {
        [Theory]
        [InlineData("Owner")]
        [InlineData("Admin")]
        [InlineData("Editor")]
        [InlineData("Publisher")]
        [InlineData("Moderator")]
        public void From_description_should_return_a_new_permissionType_successfully(string? description)
        {
            // Arrange & Act
            var actual = PermissionType.FromDescription(description);

            // Assert
            Assert.True(
                actual.IsSuccess &&
                string.Equals(actual.Value.Description, description, StringComparison.CurrentCultureIgnoreCase));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void From_id_should_return_a_new_permissionType_successfully(int id)
        {
            // Arrange & Act
            var actual = PermissionType.FromId(id);

            // Assert
            Assert.True(
                actual.IsSuccess &&
                actual.Value.Id == id);
        }
    }

    public class Given_invalid_permissionType_data
    {
        [Fact]
        public void From_description_should_return_error()
        {
            // Arrange 
            var invalidPermissionDescription = "test";

            // Arrange & Act
            var actual = PermissionType.FromDescription(invalidPermissionDescription);

            // Assert
            Assert.True(
                actual.IsFailure &&
                actual.Error.Code == "Permissions.InvalidPermissionType");
        }

        [Fact]
        public void From_id_should_return_error()
        {
            // Arrange 
            var invalidPermissionId = 123;

            // Arrange & Act
            var actual = PermissionType.FromId(invalidPermissionId);

            // Assert
            Assert.True(
                actual.IsFailure &&
                actual.Error.Code == "Permissions.InvalidPermissionId");
        }
    }
}