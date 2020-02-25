namespace CN88.Features.Tests.Reports.Criteria
{
    using CN88.Features.Reports.Criteria;
    using Xunit;

    public class GetAccountingPeriodsForAgencyCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetAccountingPeriodsForAgencyCriteria();

            // Act & Assert
            Assert.Equal("CS_AccountingPeriod_Sel", criteria.GetSettingKey());
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetAccountingPeriodsForAgencyCriteria();

            // Act & Assert
            Assert.True(criteria.IsValid());
        }
    }
}