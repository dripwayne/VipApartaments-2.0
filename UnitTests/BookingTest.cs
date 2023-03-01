using VipApartaments.Models;
using Xunit;

namespace VipApartaments.Tests
{
    public class BookingTests
    {
        [Fact]
        public void Booking_ShouldSetProperties()
        {
            // Arrange
            int expectedId = 1;
            int expectedIdClient = 2;
            int expectedIdRoom = 3;
            int expectedIdMethodOfPayment = 4;
            int expectedToPay = 100;
            bool expectedPay = false;

            // Act
            Booking booking = new Booking
            {
                Id = expectedId,
                IdClient = expectedIdClient,
                IdRoom = expectedIdRoom,
                IdMethodOfPayment = expectedIdMethodOfPayment,
                ToPay = expectedToPay,
                Pay = expectedPay
            };

            // Assert
            Assert.Equal(expectedId, booking.Id);
            Assert.Equal(expectedIdClient, booking.IdClient);
            Assert.Equal(expectedIdRoom, booking.IdRoom);
            Assert.Equal(expectedIdMethodOfPayment, booking.IdMethodOfPayment);
            Assert.Equal(expectedToPay, booking.ToPay);
            Assert.Equal(expectedPay, booking.Pay);
        }
    }
}