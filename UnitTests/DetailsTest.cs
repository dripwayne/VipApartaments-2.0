using System;
using Xunit;
using VipApartaments.Models;

namespace VipApartaments.Tests
{
    public class DetailsTests
    {
        [Fact]
        public void CanCreateDetails()
        {
            // Arrange
            var details = new Details
            {
                IdBook = 1,
                DateFrom = new DateTime(2023, 4, 1),
                DateTo = new DateTime(2023, 4, 5)
            };

            // Act
            var result = details;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.IdBook);
            Assert.Equal(new DateTime(2023, 4, 1), result.DateFrom);
            Assert.Equal(new DateTime(2023, 4, 5), result.DateTo);
        }
    }
}