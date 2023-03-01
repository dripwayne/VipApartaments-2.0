using Xunit;
using VipApartaments.Models;

namespace VipApartaments.Tests
{
    public class RoomsTests
    {
        [Fact]
        public void CanCreateRoom()
        {
            // Arrange
            var room = new Rooms
            {
                RoomType = "Deluxe Suite",
                RoomPrice = 500
            };

            // Act
            var result = room;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Deluxe Suite", result.RoomType);
            Assert.Equal(500, result.RoomPrice);
        }
    }
}
