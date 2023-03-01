using Xunit;
using VipApartaments.Models;

namespace VipApartaments.Tests
{
    public class PaymentTests
    {
        [Fact]
        public void CanCreatePayment()
        {
            // Arrange
            var payment = new Payment
            {
                PaymentMethod = "Credit Card"
            };

            // Act
            var result = payment;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Credit Card", result.PaymentMethod);
        }
    }
}