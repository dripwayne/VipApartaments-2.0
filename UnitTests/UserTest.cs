using Xunit;
using VipApartaments.Models;

namespace VipApartaments.Tests
{
    public class UserTests
    {
        [Fact]
        public void CanCreateUser()
        {
            // Arrange
            var user = new User
            {
                UserName = "testuser",
                Pass = "password123"
            };

            // Act
            var result = user;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.UserName);
            Assert.Equal("password123", result.Pass);
        }
    }
}