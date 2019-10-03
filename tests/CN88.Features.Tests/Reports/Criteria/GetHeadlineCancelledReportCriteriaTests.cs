namespace CN88.Features.Tests.Reports.Criteria
{
    using System;
    using CN88.Features.Reports.Criteria;
    using CN88.Features.Shared.Models;
    using CN88.Features.Tests.Constants;
    using Xunit;

    public class GetHeadlineCancelledReportCriteriaTests
    {
        [Theory]
        [InlineData(RoleIds.Super, "CS_HL_Rpt_Cancelled_Super")]
        [InlineData(RoleIds.Master, "CS_HL_Rpt_Cancelled_Master")]
        [InlineData(RoleIds.Agent, "CS_HL_Rpt_Cancelled_Agent")]
        [InlineData(RoleIds.Member, "CS_HL_Rpt_Cancelled_Member")]
        public void GetSettingKey_WithCorrectReportLevel_ReturnsCorrectSettingKey(byte reportLevel, string expectedSettingKey)
        {
            // Arrange
            var criteria = new GetHeadlineCancelledReportCriteria(
                123,
                1,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "0",
                0,
                0,
                3,
                Enumeration.FromValue<UserLevel>(reportLevel));

            // Act & Assert
            Assert.Equal(expectedSettingKey, criteria.GetSettingKey());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidHeadline_ReturnsFalse(int headlineId)
        {
            // Arrange
            var criteria = new GetHeadlineCancelledReportCriteria(
                headlineId,
                1,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "0",
                0,
                0,
                3,
                Enumeration.FromValue<UserLevel>(RoleIds.Super));

            // Act & Assert
            Assert.False(criteria.IsValid());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new GetHeadlineCancelledReportCriteria(
                123,
                custId,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "0",
                0,
                0,
                3,
                Enumeration.FromValue<UserLevel>(RoleIds.Master));

            // Act & Assert
            Assert.False(criteria.IsValid());
        }

        [Fact]
        public void IsValid_FromDateIsGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetHeadlineCancelledReportCriteria(
                123,
                1,
                DateTime.Now.AddDays(1),
                DateTime.Now,
                DateTime.Now,
                "0",
                0,
                0,
                3,
                Enumeration.FromValue<UserLevel>(RoleIds.Agent));

            // Act & Assert
            Assert.False(criteria.IsValid());
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetHeadlineCancelledReportCriteria(
                123,
                1,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "0",
                0,
                0,
                3,
                Enumeration.FromValue<UserLevel>(RoleIds.Member));

            // Act & Assert
            Assert.True(criteria.IsValid());
        }
    }
}