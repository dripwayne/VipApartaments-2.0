using Xunit;
using VipApartaments.Models;

namespace VipApartaments.Tests
{
    public class ClientsTests
    {
        [Fact]
        public void CanCreateClient()
        {
            // Arrange
            var client = new Clients
            {
                FirstName = "John",
                LastName = "Doe",
                Phone = "0000000000",
                Email = "johndoe@example.com",
                password = "Pa$$w0rd"
            };

            // Act
            var result = client;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("0000000000", result.Phone);
            Assert.Equal("johndoe@example.com", result.Email);
            Assert.Equal("Pa$$w0rd", result.password);
        }
    }
}