namespace CN88.Features.Tests.Reports.Criteria
{
    using CN88.Features.Reports.Criteria;
    using CN88.Features.Tests.Constants;
    using Xunit;

    public class GetCustomerPTSettingCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKeys()
        {
            // Arrange
            var criteria = new GetCustomerPTSettingCriteria(RoleIds.Super, "1,2", true);

            // Act & Assert
            Assert.Equal("CS_CustSetting_LegacyReportSetting_Sel", criteria.GetSettingKey());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidRoleId_ReturnsFalse(int roleId)
        {
            // Arrange
            var criteria = new GetCustomerPTSettingCriteria(roleId, "1,2", true);

            // Act & Assert
            Assert.False(criteria.IsValid());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValid_InvalidCustIds_ReturnsFalse(string custIds)
        {
            // Arrange
            var criteria = new GetCustomerPTSettingCriteria(RoleIds.Super, custIds, true);

            // Act & Assert
            Assert.False(criteria.IsValid());
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetCustomerPTSettingCriteria(RoleIds.Super, "1,2", true);

            // Act & Assert
            Assert.True(criteria.IsValid());
        }
    }
}