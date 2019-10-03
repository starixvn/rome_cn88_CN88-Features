namespace CN88.Features.Tests.Shared.Criteria
{
    using System;
    using CN88.Features.Shared.Criteria;
    using Xunit;

    public class CheckHeadlineBelongingByDateCiteriaTests
    {
        [Fact]
        public void GetSettingKey_ValidCriteria_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new CheckHeadlineBelongingByDateCommand(
                123,
                1,
                DateTime.Now,
                DateTime.Now);

            // Act & Assert
            Assert.Equal("CS_Customer_CheckBelongingHeadline_ByDate", criteria.GetSettingKey());
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new CheckHeadlineBelongingByDateCommand(
                123,
                1,
                DateTime.Now,
                DateTime.Now);

            // Act & Assert
            Assert.True(criteria.IsValid());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidHeadline_ReturnsFalse(int headlineId)
        {
            // Arrange
            var criteria = new CheckHeadlineBelongingByDateCommand(
                headlineId,
                1,
                DateTime.Now,
                DateTime.Now);

            // Act & Assert
            Assert.False(criteria.IsValid());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new CheckHeadlineBelongingByDateCommand(
                123,
                custId,
                DateTime.Now,
                DateTime.Now);

            // Act & Assert
            Assert.False(criteria.IsValid());
        }

        [Fact]
        public void IsValid_FromDateIsGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var criteria = new CheckHeadlineBelongingByDateCommand(
                123,
                1,
                DateTime.Now.AddSeconds(5),
                DateTime.Now);

            // Act & Assert
            Assert.False(criteria.IsValid());
        }
    }
}
