namespace CN88.Features.Site.Tests.BetLists.ViewModels
{
    using System.Linq;
    using CN88.Features.Shared.Models;
    using CN88.Features.Site.BetLists.ViewModels;
    using Xunit;

    [Trait("BetList", "BetListOfReport")]
    public class BetListOfReportViewModelTests
    {
        [Fact]
        public void BuildBreadcrumbParamByLevel_Headline_ReturnsCorrectValue()
        {
            // Arrange
            var reportPath = "ReportPath";
            var expectedValue = Enumerable.Repeat(reportPath, 4);

            // Act
            var actualValue = BetListOfReportViewModel.BuildBreadcrumbParamByLevel(reportPath, UserLevel.Headline);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void BuildBreadcrumbParamByLevel_Super_ReturnsCorrectValue()
        {
            // Arrange
            var reportPath = "ReportPath";
            var expectedValue = Enumerable.Repeat(reportPath, 4);

            // Act
            var actualValue = BetListOfReportViewModel.BuildBreadcrumbParamByLevel(reportPath, UserLevel.Super);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void BuildBreadcrumbParamByLevel_Master_ReturnsCorrectValue()
        {
            // Arrange
            var reportPath = "ReportPath";
            var expectedValue = Enumerable.Repeat(reportPath, 3);

            // Act
            var actualValue = BetListOfReportViewModel.BuildBreadcrumbParamByLevel(reportPath, UserLevel.Master);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void BuildBreadcrumbParamByLevel_Agent_ReturnsCorrectValue()
        {
            // Arrange
            var reportPath = "ReportPath";
            var expectedValue = Enumerable.Repeat(reportPath, 2);

            // Act
            var actualValue = BetListOfReportViewModel.BuildBreadcrumbParamByLevel(reportPath, UserLevel.Agent);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void BuildBreadcrumbParamByLevel_Member_ReturnsCorrectValue()
        {
            // Arrange
            var reportPath = "ReportPath";
            var expectedValue = Enumerable.Repeat(reportPath, 0);

            // Act
            var actualValue = BetListOfReportViewModel.BuildBreadcrumbParamByLevel(reportPath, UserLevel.Member);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
