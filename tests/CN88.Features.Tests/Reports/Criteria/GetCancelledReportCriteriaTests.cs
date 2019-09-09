namespace CN88.Features.Tests.Reports.Criteria
{
    using System;
    using CN88.Features.Reports.Criteria;
    using CN88.Features.Shared.Models;
    using CN88.Features.Tests.Constants;
    using Xunit;

    public class GetCancelledReportCriteriaTests
    {
        [Theory]
        [InlineData(RoleIds.Super, "CS_Rpt_Cancelled_Super")]
        [InlineData(RoleIds.Master, "CS_Rpt_Cancelled_Master")]
        [InlineData(RoleIds.Agent, "CS_Rpt_Cancelled_Agent")]
        public void GetSettingKey_WithCorrectReportLevel_ReturnsCorrectSettingKey(byte reportLevel, string expectedSettingKey)
        {
            // Arrange
            var criteria = new GetCancelledReportCriteria(
                custId: 1,
                subAccId: 0,
                isOwner: false,
                isAllDownline: false,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                archiveDate: DateTime.Now,
                moveBetsDay: DateTime.Now,
                sportGroupIds: "0",
                betTypeId: 0,
                isLive: 0,
                reportType: 3,
                reportLevel: Enumeration.FromValue<UserLevel>(reportLevel));

            // Act & Assert
            Assert.Equal(expectedSettingKey, criteria.GetSettingKey());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new GetCancelledReportCriteria(
                custId: custId,
                subAccId: 0,
                isOwner: false,
                isAllDownline: false,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                archiveDate: DateTime.Now,
                moveBetsDay: DateTime.Now,
                sportGroupIds: "0",
                betTypeId: 0,
                isLive: 0,
                reportType: 3,
                reportLevel: Enumeration.FromValue<UserLevel>(RoleIds.Super));

            // Act & Assert
            Assert.False(criteria.IsValid());
        }

        [Fact]
        public void IsValid_FromDateIsGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetCancelledReportCriteria(
                custId: 1,
                subAccId: 0,
                isOwner: false,
                isAllDownline: false,
                fromDate: DateTime.Now.AddDays(1),
                toDate: DateTime.Now,
                archiveDate: DateTime.Now,
                moveBetsDay: DateTime.Now,
                sportGroupIds: "0",
                betTypeId: 0,
                isLive: 0,
                reportType: 3,
                reportLevel: Enumeration.FromValue<UserLevel>(RoleIds.Super));

            // Act & Assert
            Assert.False(criteria.IsValid());
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetCancelledReportCriteria(
                custId: 1,
                subAccId: 0,
                isOwner: false,
                isAllDownline: false,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                archiveDate: DateTime.Now,
                moveBetsDay: DateTime.Now,
                sportGroupIds: "0",
                betTypeId: 0,
                isLive: 0,
                reportType: 3,
                reportLevel: Enumeration.FromValue<UserLevel>(RoleIds.Super));

            // Act & Assert
            Assert.True(criteria.IsValid());
        }
    }
}