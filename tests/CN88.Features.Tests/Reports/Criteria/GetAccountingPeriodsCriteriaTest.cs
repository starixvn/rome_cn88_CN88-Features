namespace CN88.Features.Tests.Reports.Criteria
{
    using CN88.Features.Reports.Criteria;
    using Xunit;

    public class GetAccountingPeriodsCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetAccountingPeriodsCriteria();

            // Act & Assert
            Assert.Equal("CS_AccountingPeriod_Sel", criteria.GetSettingKey());
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetAccountingPeriodsCriteria();

            // Act & Assert
            Assert.True(criteria.IsValid());
        }
    }
}