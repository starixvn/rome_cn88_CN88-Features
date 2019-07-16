namespace CN88.Features.Reports.Models
{
    public class AssignedDownlinesCount
    {
        public AssignedDownlinesCount()
        {
        }

        public AssignedDownlinesCount(
            int totalAssignedAccounts,
            int totalDirectDownlines)
        {
            TotalAssignedAccounts = totalAssignedAccounts;
            TotalDirectDownlines = totalDirectDownlines;
        }

        public int TotalAssignedAccounts { get; }

        public int TotalDirectDownlines { get; }
    }
}